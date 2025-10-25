using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using ROP;
using AgoraCertaminaBack.Models.General;


namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class GetDataSchemaById( IMongoRepository<SchemaContest> _mongoRepository, IMongoRepository<Tenant> _tenantRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<DataSchemaDTO>> Execute(string schemaId)
        {
            var schema = await _mongoRepository.FindOneAsync(
                filter => filter.Id == schemaId &&
                filter.OrganizationId == _userRequest.OrganizationId &&
                filter.IsActive);

            if (schema == null)
                return Result.NotFound<DataSchemaDTO>("Schema not found");

            var customer = await _tenantRepository.FindOneAsync( filter => filter.Id == _userRequest.OrganizationId);

            if (customer == null)
                return Result.Failure<DataSchemaDTO>("Customer not found");

            var allFields = new List<Field>();

            if (schema.SchemaFields != null)
            {
                allFields.AddRange(schema.SchemaFields.Where(f => f.IsActive));
            }
            
            var catalogIds = allFields
                .Where(field => !string.IsNullOrEmpty(field.CatalogId))
                .Select(field => field.CatalogId!)
                .Distinct()
                .ToList();

            if (!catalogIds.Any())
            {
                var detailDTO = schema.ToDataSchemaDTO(allFields, new List<CatalogReferenceDTO>());
                return Result.Success(detailDTO);
            }

            var catalogs = catalogIds
                .Select(catalogId => customer.Catalogs?
                    .FirstOrDefault(catalog => catalog.Id == catalogId && catalog.IsActive))
                .Where(catalog => catalog != null)
                .Select(catalog => new CatalogReferenceDTO
                {
                    CatalogId = catalog!.Id,
                    CatalogName = catalog.Name,
                    Values = catalog.Values?.Select(value => value.AsString).ToList() ?? new List<string>()
                })
                .ToList();

            var detailDTOFinal = schema.ToDataSchemaDTO(allFields, catalogs);
            return Result.Success(detailDTOFinal);
        }
    }
}