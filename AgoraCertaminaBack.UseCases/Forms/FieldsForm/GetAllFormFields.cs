using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Forms.FieldsForm
{
    public class GetAllFormFields(IMongoRepository<Form> _mongoRepository)
    {
        public async Task<Result<List<CustomField>>> Execute(string formId)
        {
            var formFields = await _mongoRepository.FilterByAsync(
                filter => filter.Id == formId,
                project => project.FormFields
                  .Where(field => field.IsActive)
                  .OrderBy(field => field.Order)
                  .ThenBy(field => field.CreatedAt)
                  .ToList()
            );

            return formFields;
        }
    }
}