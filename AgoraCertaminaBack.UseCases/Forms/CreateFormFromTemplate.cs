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
            var factory = _templateProvider.GetFactory(request.TemplateId);
            var templateData = factory.GetTemplateData();

            var createdTags = await CreateTagsInTenant(tenant, templateData.Tags);
            var catalogIdMapping = await CreateCatalogsInTenant(tenant, templateData.Catalogs);

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
                    IsActive = true,
                    CatalogId = null
                };

                if (fieldDto.Type == FieldTypeEnum.CustomCatalog && !string.IsNullOrEmpty(fieldDto.CatalogId))
                {
                    string catalogName = ExtractCatalogNameFromPlaceholder(fieldDto.CatalogId);

                    var matchingCatalog = catalogIdMapping.FirstOrDefault(kvp =>
                        kvp.Key.Equals(catalogName, StringComparison.OrdinalIgnoreCase)
                    );

                    if (!string.IsNullOrEmpty(matchingCatalog.Key))
                    {
                        customField.CatalogId = matchingCatalog.Value;
                    }
                }

                formFields.Add(customField);
            }

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
        
        private string ExtractCatalogNameFromPlaceholder(string placeholder)
        {
            if (string.IsNullOrEmpty(placeholder) || !placeholder.EndsWith("_PLACEHOLDER"))
            {
                return placeholder;
            }

            string nameWithUnderscores = placeholder.Replace("_PLACEHOLDER", "");
            var words = nameWithUnderscores.Split('_');

            var titleCaseWords = words
                .Where(w => !string.IsNullOrEmpty(w))
                .Select(word =>
                {
                    if (word.Length == 1) return word;

                    string lowerWord = word.ToLower(System.Globalization.CultureInfo.CurrentCulture);
                    return char.ToUpper(lowerWord[0]) + lowerWord.Substring(1);
                });

            return string.Join(" ", titleCaseWords);
        }

        private async Task<List<Tag>> CreateTagsInTenant(Tenant tenant, List<Models.DTOs.Form.Templates.CustomTagRequest> tagRequests)
        {
            var createdTags = new List<Tag>();

            foreach (var tagRequest in tagRequests)
            {
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
            var catalogMapping = new Dictionary<string, string>();

            foreach (var catalogRequest in catalogRequests)
            {
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
                    var newCatalog = catalogRequest.ToCustomCatalog();
                    tenant.Catalogs.Add(newCatalog);
                    catalogMapping[catalogRequest.Name] = newCatalog.Id;
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