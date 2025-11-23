using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Forms.Templates;
using AgoraCertaminaBack.UseCases.Tenants;
using ROP;
using System.Net;
using MongoDB.Bson;

namespace AgoraCertaminaBack.UseCases.Forms
{
    /// <summary>
    /// Caso de uso que implementa la creación de formularios usando el patrón Method Factory.
    /// Coordina la creación de tags, catálogos y campos basados en plantillas predefinidas.
    /// </summary>
    public class CreateFormFromTemplate
    {
        private readonly IMongoRepository<Form> _formRepository;
        private readonly IMongoRepository<Tenant> _tenantRepository;
        private readonly GetByIdTenant _getByIdTenant;
        private readonly UserRequestContext _userRequest;
        private readonly FormTemplateFactoryProvider _templateProvider;

        public CreateFormFromTemplate(
            IMongoRepository<Form> formRepository,
            IMongoRepository<Tenant> tenantRepository,
            GetByIdTenant getByIdTenant,
            UserRequestContext userRequest,
            FormTemplateFactoryProvider templateProvider)
        {
            _formRepository = formRepository;
            _tenantRepository = tenantRepository;
            _getByIdTenant = getByIdTenant;
            _userRequest = userRequest;
            _templateProvider = templateProvider;
        }

        public async Task<Result<string>> Execute(CreateFormFromTemplateRequest request)
        {
            return await _getByIdTenant.Execute(_userRequest.OrganizationId)
                .Bind(tenant => ValidateUniqueName(tenant, request.FormName))
                .Bind(tenant => CreateFormWithTemplate(tenant, request));
        }

        private async Task<Result<Tenant>> ValidateUniqueName(Tenant tenant, string formName)
        {
            bool formExists = await _formRepository.ExistsAsync(f =>
                f.IsActive &&
                f.OrganizationId == tenant.Id &&
                f.FormName == formName
            );

            if (formExists)
                return Result.Failure<Tenant>("A form with this name already exists");

            return tenant.Success();
        }

        private async Task<Result<string>> CreateFormWithTemplate(Tenant tenant, CreateFormFromTemplateRequest request)
        {
            // 1. Obtener la fábrica apropiada según la categoría (Method Factory Pattern)
            var factory = _templateProvider.GetFactory(request.TemplateCategory);
            var templateData = factory.GetTemplateData();

            // 2. Crear Tags en el tenant y obtener sus IDs
            var createdTags = await CreateTagsInTenant(tenant, templateData.Tags);

            // 3. Crear Catálogos en el tenant y obtener mapeo de nombres a IDs reales
            var catalogIdMapping = await CreateCatalogsInTenant(tenant, templateData.Catalogs);

            // 4. Crear lista de CustomField con los CatalogIds correctos
            var formFields = new List<CustomField>();

            foreach (var fieldDto in templateData.Fields)
            {
                var customField = new CustomField
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = fieldDto.Name,
                    Type = fieldDto.Type,
                    IsRequired = fieldDto.IsRequired,
                    Order = fieldDto.Order,
                    StaticValue = fieldDto.StaticValue ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // Si el campo es de tipo CustomCatalog y tiene un placeholder, reemplazarlo
                if (fieldDto.Type == FieldTypeEnum.CustomCatalog && !string.IsNullOrEmpty(fieldDto.CatalogId))
                {
                    // El placeholder viene en formato "NOMBRE_CATALOGO_PLACEHOLDER"
                    // Necesitamos buscar el catálogo por nombre
                    string catalogName = ExtractCatalogNameFromPlaceholder(fieldDto.CatalogId);

                    if (catalogIdMapping.ContainsKey(catalogName))
                    {
                        customField.CatalogId = catalogIdMapping[catalogName];
                    }
                    else
                    {
                        // Si no se encuentra el catálogo, dejar el campo sin catálogo
                        customField.CatalogId = null;
                    }
                }

                formFields.Add(customField);
            }

            // 5. Crear el formulario con todos los datos
            var newForm = new Form
            {
                OrganizationId = tenant.Id,
                TenantName = tenant.TenantName,
                FormName = request.FormName,
                Tags = createdTags,
                FormFields = formFields,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _formRepository.InsertOneAsync(newForm);

            return newForm.Id.Success(HttpStatusCode.Created);
        }

        /// <summary>
        /// Extrae el nombre del catálogo desde un placeholder
        /// Ejemplo: "DEPARTAMENTOS_PLACEHOLDER" -> "Departamentos"
        /// </summary>
        private string ExtractCatalogNameFromPlaceholder(string placeholder)
        {
            if (string.IsNullOrEmpty(placeholder) || !placeholder.EndsWith("_PLACEHOLDER"))
                return placeholder;

            // Remover "_PLACEHOLDER" del final
            string nameWithUnderscores = placeholder.Replace("_PLACEHOLDER", "");

            // Convertir de SNAKE_CASE a Title Case
            // "TIPO_CONTRATO" -> "Tipo de Contrato"
            var words = nameWithUnderscores.Split('_');
            var titleCaseWords = words.Select(word =>
                char.ToUpper(word[0]) + word.Substring(1).ToLower()
            );

            return string.Join(" ", titleCaseWords);
        }

        /// <summary>
        /// Crea los tags de la plantilla en el tenant
        /// </summary>
        private async Task<List<Tag>> CreateTagsInTenant(Tenant tenant, List<Models.DTOs.Form.Templates.CustomTagRequest> tagRequests)
        {
            var createdTags = new List<Tag>();

            foreach (var tagRequest in tagRequests)
            {
                // Verificar si ya existe un tag con el mismo nombre
                var existingTag = tenant.Tags.FirstOrDefault(t =>
                    t.IsActive &&
                    t.Name.Equals(tagRequest.Name, StringComparison.CurrentCultureIgnoreCase)
                );

                if (existingTag != null)
                {
                    createdTags.Add(existingTag);
                }
                else
                {
                    // Crear nuevo tag
                    var newTag = new Tag
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = tagRequest.Name,
                        Color = tagRequest.Color,
                        Category = tagRequest.Category,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    tenant.Tags.Add(newTag);
                    createdTags.Add(newTag);
                }
            }

            // Actualizar el tenant con los nuevos tags
            if (tagRequests.Any())
            {
                await _tenantRepository.ReplaceOneAsync(tenant);
            }

            return createdTags;
        }

        /// <summary>
        /// Crea los catálogos de la plantilla en el tenant y retorna mapeo de nombres a IDs
        /// </summary>
        private async Task<Dictionary<string, string>> CreateCatalogsInTenant(
            Tenant tenant,
            List<Models.DTOs.CustomCatalog.CreateCustomCatalogRequest> catalogRequests)
        {
            var catalogMapping = new Dictionary<string, string>();

            foreach (var catalogRequest in catalogRequests)
            {
                // Verificar si ya existe un catálogo con el mismo nombre
                var existingCatalog = tenant.Catalogs.FirstOrDefault(c =>
                    c.IsActive &&
                    c.Name.Equals(catalogRequest.Name, StringComparison.CurrentCultureIgnoreCase)
                );

                if (existingCatalog != null)
                {
                    catalogMapping[catalogRequest.Name] = existingCatalog.Id;
                }
                else
                {
                    // Crear nuevo catálogo
                    var newCatalog = catalogRequest.ToCustomCatalog();
                    tenant.Catalogs.Add(newCatalog);
                    catalogMapping[catalogRequest.Name] = newCatalog.Id;
                }
            }

            // Actualizar el tenant con los nuevos catálogos
            if (catalogRequests.Any())
            {
                await _tenantRepository.ReplaceOneAsync(tenant);
            }

            return catalogMapping;
        }
    }
}