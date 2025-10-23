using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class DeleteFormById(IMongoRepository<Form> _mongoRepository, GetByIdForm _getByIdSchema)
    {
        public async Task<Result<Unit>> Execute(string formId)
        {
            return await _getByIdSchema.Execute(formId)
               .Bind(form => DeleteSchema(form));
        }

        public async Task<Result<Unit>> DeleteSchema(Form form)
        {
            form.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(form);

            return Result.Success();
        }
    }
}
