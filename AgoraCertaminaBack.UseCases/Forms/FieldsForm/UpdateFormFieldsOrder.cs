using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Forms.FieldsForm
{
    public class UpdateFormFieldsOrder(IMongoRepository<Form> _mongoRepository, GetByIdForm _getByIdForm)
    {
        public async Task<Result<bool>> Execute(string formId, List<ReorderFormFieldsRequest> request)
        {
            return await _getByIdForm.Execute(formId)
                .Bind(form => ApplyOrder(form, request));
        }

        private async Task<Result<bool>> ApplyOrder(Form form, List<ReorderFormFieldsRequest> request)
        {
            var orderById = request.ToDictionary(x => x.Id, x => x.Order);

            // esto asegura que no se ordenen campos inactivos o que no vinieron en el request
            foreach (var f in form.FormFields.Where(ff => ff.IsActive && orderById.ContainsKey(ff.Id)))
                f.Order = orderById[f.Id];

            await _mongoRepository.ReplaceOneAsync(form);

            return true.Success();
        }
    }
}
