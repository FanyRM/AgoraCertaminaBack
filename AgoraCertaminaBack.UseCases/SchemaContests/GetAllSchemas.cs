using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class GetAllSchemas(IMongoRepository<SchemaContest> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<SchemaContestDTOs>>> Execute()
        {
            var schemas = await _mongoRepository.FilterByAsync(filter =>
            filter.OrganizationId == _userRequest.OrganizationId && filter.IsActive);

            var schemaDTO = schemas.Select(schema => schema.ToSchemaContestDTO()).ToList();

            return schemaDTO;
        }
    }
}