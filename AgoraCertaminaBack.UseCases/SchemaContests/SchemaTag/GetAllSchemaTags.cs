using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests.SchemaTag
{
    public class GetAllSchemaTags(IMongoRepository<SchemaContest> _mongoRepository)
    {
        public async Task<Result<List<Tag>>> Execute(string schemaId)
        {
            var customTags = await _mongoRepository.FilterByAsync(
                filter => filter.Id == schemaId,
                project => project.Tags.Where(tag => tag.IsActive).ToList());

            return customTags;
        }
    }
}