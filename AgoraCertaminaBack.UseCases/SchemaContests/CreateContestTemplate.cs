using System.Net;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.SchemaContests.Template;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Tenants;
using MongoDB.Bson;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class CreateContestTemplate(
        IMongoRepository<SchemaContest> _mongoRepository,
        IMongoRepository<Tenant> _tenantRepository,
        GetByIdTenant _getByIdTenant,
        UserRequestContext _userRequestContext,
        ContestTemplateFactoryProvider _contestTemplateFactoryProvider)
    {
        public async Task<Result<string>> Execute(CreateContestTemplateRequest request)
        {
            return await _getByIdTenant.Execute(_userRequestContext.OrganizationId)
                .Bind(tenant => ValidateUniqueName(tenant, request.ContestName))
                .Bind(tenant => CreateContestWithTemplate(tenant, request));
        }

        private async Task<Result<Tenant>> ValidateUniqueName(Tenant tenant, string contestName)
        {
            bool contestExists = await _mongoRepository.ExistsAsync(f =>
                f.IsActive &&
                f.OrganizationId == tenant.Id &&
                f.SchemaName == contestName
            );

            if (contestExists)
                return Result.Failure<Tenant>("A schema contest with this name already exists");

            return tenant.Success();
        }

        private async Task<Result<string>> CreateContestWithTemplate(Tenant tenant, CreateContestTemplateRequest request)
        {
            var factory = _contestTemplateFactoryProvider.GetFactory(request.ContestCategory);
            var templateData = factory.GetContestTemplateData();

            var createdTags = await CreateTagsInTenant(tenant, templateData.Tags);

            var catalogMapping = await CreateCatalogsInTenant(tenant, templateData.Catalogs);
            
            var contestFields = new List<Field>();

            foreach (var fieldDto in templateData.Fields)
            {
                var field = new Field
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = fieldDto.Name,
                    Type = fieldDto.Type,
                    IsRequired = fieldDto.IsRequired,
                    IsBase = false,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    CatalogId = null
                };

                if (fieldDto.Type == FieldTypeEnum.CustomCatalog)
                {

                    if (!string.IsNullOrEmpty(fieldDto.CatalogId))
                    {

                        string catalogName = ExtractCatalogNameFromPlaceholder(fieldDto.CatalogId);

                        var matchingCatalog = catalogMapping.FirstOrDefault(kvp =>
                            kvp.Key.Equals(catalogName, StringComparison.OrdinalIgnoreCase)
                        );

                        if (!string.IsNullOrEmpty(matchingCatalog.Key))
                        {
                            field.CatalogId = matchingCatalog.Value;
                        }
                    }
                }

                contestFields.Add(field);
            }

            var unresolvedFields = contestFields
                .Where(f => f.Type == FieldTypeEnum.CustomCatalog && string.IsNullOrEmpty(f.CatalogId))
                .ToList();


            var newContest = new SchemaContest
            {
                OrganizationId = tenant.Id,
                OrganizationName = tenant.TenantName,
                SchemaName = request.ContestName,
                SchemaFields = contestFields,
                Tags = createdTags,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _mongoRepository.InsertOneAsync(newContest);

            return newContest.Id.Success(HttpStatusCode.Created);
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

            string result = string.Join(" ", titleCaseWords);

            return result;
        }

        private async Task<List<Tag>> CreateTagsInTenant(Tenant tenant, List<CustomTagRequest> tagRequests)
        {
            var output = new List<Tag>();

            foreach (var tagReq in tagRequests)
            {
                var existing = tenant.Tags.FirstOrDefault(t =>
                    t.IsActive &&
                    t.Name.Equals(tagReq.Name, StringComparison.OrdinalIgnoreCase)
                );

                if (existing != null)
                {
                    output.Add(existing);
                }
                else
                {
                    var newTag = new Tag
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = tagReq.Name,
                        Color = tagReq.Color,
                        Category = tagReq.Category,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    tenant.Tags.Add(newTag);
                    output.Add(newTag);
                }
            }

            if (tagRequests.Any())
            {
                await _tenantRepository.ReplaceOneAsync(tenant);
            }

            return output;
        }

        private async Task<Dictionary<string, string>> CreateCatalogsInTenant(
            Tenant tenant,
            List<CreateCustomCatalogRequest> catalogRequests)
        {
            var mapping = new Dictionary<string, string>();

            foreach (var req in catalogRequests)
            {

                var existing = tenant.Catalogs.FirstOrDefault(c =>
                    c.IsActive &&
                    c.Name.Equals(req.Name, StringComparison.OrdinalIgnoreCase)
                );

                if (existing != null)
                {
                    mapping[req.Name] = existing.Id;
                }
                else
                {
                    var newCatalog = req.ToCustomCatalog();
                    tenant.Catalogs.Add(newCatalog);
                    mapping[req.Name] = newCatalog.Id;
                }
            }

            if (catalogRequests.Any())
            {
                await _tenantRepository.ReplaceOneAsync(tenant);
            }

            return mapping;
        }
    }
}
