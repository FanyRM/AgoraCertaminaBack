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
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine($"[TEMPLATE] Iniciando creación: {request.TemplateCategory}");
            Console.WriteLine(new string('=', 80));

            // 1. Obtener la fábrica apropiada
            var factory = _templateProvider.GetFactory(request.TemplateCategory);
            var templateData = factory.GetTemplateData();

            Console.WriteLine($"\n[TEMPLATE] Plantilla cargada:");
            Console.WriteLine($"  Tags: {templateData.Tags.Count}");
            Console.WriteLine($"  Catálogos: {templateData.Catalogs.Count}");
            Console.WriteLine($"  Campos: {templateData.Fields.Count}");

            // 2. Crear Tags
            var createdTags = await CreateTagsInTenant(tenant, templateData.Tags);

            // 3. Crear Catálogos
            var catalogIdMapping = await CreateCatalogsInTenant(tenant, templateData.Catalogs);

            Console.WriteLine($"\n[MAPEO] Catálogos creados: {catalogIdMapping.Count}");
            foreach (var kvp in catalogIdMapping)
            {
                Console.WriteLine($"  '{kvp.Key}' -> {kvp.Value}");
            }

            // 4. Verificar enum CustomCatalog
            Console.WriteLine($"\n[ENUM] Valor de CustomCatalog: {(int)FieldTypeEnum.CustomCatalog}");
            Console.WriteLine($"[ENUM] CustomCatalog == 8: {(int)FieldTypeEnum.CustomCatalog == 8}");

            // 5. Crear campos
            var formFields = new List<CustomField>();

            foreach (var fieldDto in templateData.Fields)
            {
                Console.WriteLine($"\n{new string('-', 60)}");
                Console.WriteLine($"[CAMPO] #{fieldDto.Order}: '{fieldDto.Name}'");
                Console.WriteLine($"[CAMPO] Type (enum): {fieldDto.Type}");
                Console.WriteLine($"[CAMPO] Type (int): {(int)fieldDto.Type}");
                Console.WriteLine($"[CAMPO] CatalogId DTO: '{fieldDto.CatalogId ?? "NULL"}'");
                Console.WriteLine($"[CAMPO] ¿Es CustomCatalog? {fieldDto.Type == FieldTypeEnum.CustomCatalog}");
                Console.WriteLine($"[CAMPO] CustomCatalog value: {(int)FieldTypeEnum.CustomCatalog}");

                var customField = new CustomField
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = fieldDto.Name,
                    Type = fieldDto.Type,
                    IsRequired = fieldDto.IsRequired,
                    Order = fieldDto.Order,
                    StaticValue = fieldDto.StaticValue ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    CatalogId = null // Inicializar en null
                };

                // Verificación del tipo
                if (fieldDto.Type == FieldTypeEnum.CustomCatalog)
                {
                    Console.WriteLine($"[CAMPO] → ENTRÓ al if de CustomCatalog ✓");

                    if (!string.IsNullOrEmpty(fieldDto.CatalogId))
                    {
                        Console.WriteLine($"[CAMPO] → CatalogId NO vacío: '{fieldDto.CatalogId}'");

                        string catalogName = ExtractCatalogNameFromPlaceholder(fieldDto.CatalogId);
                        Console.WriteLine($"[CAMPO] → Nombre extraído: '{catalogName}'");

                        Console.WriteLine($"[CAMPO] → Buscando en mapeo...");
                        Console.WriteLine($"[CAMPO] → Mapeo tiene {catalogIdMapping.Count} entradas");

                        // Búsqueda case-insensitive
                        var matchingCatalog = catalogIdMapping.FirstOrDefault(kvp =>
                            kvp.Key.Equals(catalogName, StringComparison.OrdinalIgnoreCase)
                        );

                        if (!string.IsNullOrEmpty(matchingCatalog.Key))
                        {
                            customField.CatalogId = matchingCatalog.Value;
                            Console.WriteLine($"[CAMPO] → ✓✓✓ ASIGNADO: {matchingCatalog.Value}");
                        }
                        else
                        {
                            Console.WriteLine($"[CAMPO] → ✗✗✗ NO ENCONTRADO en mapeo");
                            Console.WriteLine($"[CAMPO] → Catálogos disponibles:");
                            foreach (var cat in catalogIdMapping)
                            {
                                Console.WriteLine($"[CAMPO]    - '{cat.Key}'");
                                bool match = cat.Key.Equals(catalogName, StringComparison.OrdinalIgnoreCase);
                                Console.WriteLine($"[CAMPO]      ¿Coincide con '{catalogName}'? {match}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[CAMPO] → ✗ CatalogId está vacío o null");
                    }
                }
                else
                {
                    Console.WriteLine($"[CAMPO] → NO entró al if de CustomCatalog ✗");
                    Console.WriteLine($"[CAMPO] → fieldDto.Type: {fieldDto.Type} ({(int)fieldDto.Type})");
                    Console.WriteLine($"[CAMPO] → FieldTypeEnum.CustomCatalog: {FieldTypeEnum.CustomCatalog} ({(int)FieldTypeEnum.CustomCatalog})");
                    Console.WriteLine($"[CAMPO] → Son iguales: {fieldDto.Type == FieldTypeEnum.CustomCatalog}");
                }

                Console.WriteLine($"[CAMPO] CatalogId FINAL: '{customField.CatalogId ?? "NULL"}'");
                formFields.Add(customField);
            }

            Console.WriteLine($"\n{new string('-', 60)}");

            // 6. Validar campos no resueltos
            var unresolvedFields = formFields
                .Where(f => f.Type == FieldTypeEnum.CustomCatalog && string.IsNullOrEmpty(f.CatalogId))
                .ToList();

            if (unresolvedFields.Any())
            {
                Console.WriteLine($"\n[ERROR] Campos CustomCatalog sin CatalogId: {unresolvedFields.Count}");
                foreach (var field in unresolvedFields)
                {
                    Console.WriteLine($"  - {field.Name}");
                }
            }

            // 7. Crear formulario
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

            Console.WriteLine($"\n[TEMPLATE] Guardando formulario en BD...");
            await _formRepository.InsertOneAsync(newForm);
            Console.WriteLine($"[TEMPLATE] ✓ Formulario creado: {newForm.Id}");
            Console.WriteLine(new string('=', 80) + "\n");

            return newForm.Id.Success(HttpStatusCode.Created);
        }

        private string ExtractCatalogNameFromPlaceholder(string placeholder)
        {
            Console.WriteLine($"    [EXTRACT] Input: '{placeholder}'");

            if (string.IsNullOrEmpty(placeholder) || !placeholder.EndsWith("_PLACEHOLDER"))
            {
                Console.WriteLine($"    [EXTRACT] No es placeholder válido");
                return placeholder;
            }

            string nameWithUnderscores = placeholder.Replace("_PLACEHOLDER", "");
            Console.WriteLine($"    [EXTRACT] Sin _PLACEHOLDER: '{nameWithUnderscores}'");

            var words = nameWithUnderscores.Split('_');
            Console.WriteLine($"    [EXTRACT] Palabras: [{string.Join(", ", words)}]");

            // SOLUCIÓN: Usar CultureInfo.CurrentCulture en lugar de ToLower() directo
            var titleCaseWords = words
                .Where(w => !string.IsNullOrEmpty(w))
                .Select(word =>
                {
                    if (word.Length == 1) return word;

                    // Preservar acentos y caracteres especiales
                    // Usar ToLower con cultura invariante y luego capitalizar
                    string lowerWord = word.ToLower(System.Globalization.CultureInfo.CurrentCulture);
                    return char.ToUpper(lowerWord[0]) + lowerWord.Substring(1);
                });

            string result = string.Join(" ", titleCaseWords);
            Console.WriteLine($"    [EXTRACT] Output: '{result}'");

            return result;
        }

        private async Task<List<Tag>> CreateTagsInTenant(Tenant tenant, List<Models.DTOs.Form.Templates.CustomTagRequest> tagRequests)
        {
            Console.WriteLine($"\n[TAGS] Procesando {tagRequests.Count} tag(s)...");
            var createdTags = new List<Tag>();

            foreach (var tagRequest in tagRequests)
            {
                var existingTag = tenant.Tags.FirstOrDefault(t =>
                    t.IsActive &&
                    t.Name.Equals(tagRequest.Name, StringComparison.CurrentCultureIgnoreCase)
                );

                if (existingTag != null)
                {
                    Console.WriteLine($"[TAGS]   ↻ Reutilizado: '{existingTag.Name}'");
                    createdTags.Add(existingTag);
                }
                else
                {
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
                    Console.WriteLine($"[TAGS]   + Creado: '{newTag.Name}' ({newTag.Id})");
                }
            }

            if (tagRequests.Any())
            {
                await _tenantRepository.ReplaceOneAsync(tenant);
            }

            return createdTags;
        }

        private async Task<Dictionary<string, string>> CreateCatalogsInTenant(
            Tenant tenant,
            List<Models.DTOs.CustomCatalog.CreateCustomCatalogRequest> catalogRequests)
        {
            Console.WriteLine($"\n[CATALOGS] Procesando {catalogRequests.Count} catálogo(s)...");
            var catalogMapping = new Dictionary<string, string>();

            foreach (var catalogRequest in catalogRequests)
            {
                Console.WriteLine($"[CATALOGS] Catálogo: '{catalogRequest.Name}'");

                var existingCatalog = tenant.Catalogs.FirstOrDefault(c =>
                    c.IsActive &&
                    c.Name.Equals(catalogRequest.Name, StringComparison.CurrentCultureIgnoreCase)
                );

                if (existingCatalog != null)
                {
                    catalogMapping[catalogRequest.Name] = existingCatalog.Id;
                    Console.WriteLine($"[CATALOGS]   ↻ Reutilizado: {existingCatalog.Id}");
                }
                else
                {
                    var newCatalog = catalogRequest.ToCustomCatalog();
                    tenant.Catalogs.Add(newCatalog);
                    catalogMapping[catalogRequest.Name] = newCatalog.Id;
                    Console.WriteLine($"[CATALOGS]   + Creado: {newCatalog.Id}");
                }
            }

            if (catalogRequests.Any())
            {
                await _tenantRepository.ReplaceOneAsync(tenant);
            }

            return catalogMapping;
        }
    }
}