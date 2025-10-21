using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using System.Net;
using AgoraCertaminaBack.Models.DTOs.Form.FormFields;

namespace AgoraCertaminaBack.UseCases.Forms.FieldsForm
{
    public class UpdateFormField(IMongoRepository<Form> _mongoRepository, GetByIdForm _getById)
    {
        public async Task<Result<CustomField>> Execute(string formId, UpdateFormFieldRequest request)
        {
            return await _getById.Execute(formId)
                .Bind(form => UpdateSchemaField(form, request));
        }

        public async Task<Result<CustomField>> UpdateSchemaField(Form form, UpdateFormFieldRequest request)
        {
            var fieldToUpdate = form.FormFields.FirstOrDefault(field => field.Id == request.Id);
            if (fieldToUpdate == null)
                return Result.NotFound<CustomField>("Field not found");

            fieldToUpdate.Name = request.Name;
            fieldToUpdate.Type = request.Type;
            fieldToUpdate.IsRequired = request.IsRequired;
            fieldToUpdate.Section = request.Section;
            fieldToUpdate.SubSection = request.SubSection;
            fieldToUpdate.StaticValue = request.StaticValue;
           
            await _mongoRepository.ReplaceOneAsync(form);
            return fieldToUpdate.Success(HttpStatusCode.OK);
        }
    }
}