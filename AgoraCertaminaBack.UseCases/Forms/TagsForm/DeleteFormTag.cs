using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Forms.TagsForm
{
    public class DeleteFormTag(IMongoRepository<Form> _mongoRepository, GetByIdForm _getByIdSchema)
    {
        public async Task<Result<Unit>> Execute(string formId, string schemaTagId)
        {
            return await _getByIdSchema.Execute(formId)
                .Bind(form => DeleteTag(form, schemaTagId));
        }

        public async Task<Result<Unit>> DeleteTag(Form form, string schemaTagId)
        {
            var tagToDelete = form.Tags.FirstOrDefault(tag => tag.Id == schemaTagId);

            if (tagToDelete == null)
            {
                return Result.NotFound<Unit>("Tag not found");
            }

            if (!tagToDelete.IsActive)
            {
                return Result.NotFound<Unit>("Already deleted");
            }

            tagToDelete.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(form);

            return Result.Success();
        }
    }
}