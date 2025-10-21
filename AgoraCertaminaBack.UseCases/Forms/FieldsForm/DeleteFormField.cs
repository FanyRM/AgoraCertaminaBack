using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Forms.FieldsForm
{
    public class DeleteFormField(IMongoRepository<Form> _mongoRepository, GetByIdForm _getById)
    {
        public async Task<Result<Unit>> Execute(string formId, string schemaFieldId)
        {
            return await _getById.Execute(formId)
                .Bind(form => DeleteField(form, schemaFieldId));
        }

        public async Task<Result<Unit>> DeleteField(Form form, string schemaFieldId)
        {
            var fieldToDelete = form.FormFields.FirstOrDefault(field => field.Id == schemaFieldId);

            if (fieldToDelete == null)
            {
                return Result.NotFound<Unit>("Field not found");
            }

            if (!fieldToDelete.IsActive)
            {
                return Result.NotFound<Unit>("Already deleted");
            }

            fieldToDelete.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(form);

            return Result.Success();
        }
    }
}