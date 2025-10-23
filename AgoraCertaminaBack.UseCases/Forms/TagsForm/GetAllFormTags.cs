//using ROP;
//using AgoraCertaminaBack.Data.Repository;
//using AgoraCertaminaBack.Models.Entities;

//namespace AgoraCertaminaBack.UseCases.Forms.TagsForm
//{
//    public class GetAllFormTags(IMongoRepository<Form> _mongoRepository)
//    {
//        public async Task<Result<List<CustomTag>>> Execute(string formId)
//        {
//            var customTags = await _mongoRepository.FilterByAsync(
//                filter => filter.Id == formId,
//                project => project.Tags.Where(tag => tag.IsActive).ToList());

//            return customTags;
//        }
//    }
//}