//using ROP;
//using AgoraCertaminaBack.Data.Repository;
//using AgoraCertaminaBack.Models.DTOs.Form.FormTag;
//using AgoraCertaminaBack.Models.Entities;
//using AgoraCertaminaBack.Models.Mappers;
//using System.Net;

//namespace AgoraCertaminaBack.UseCases.Forms.TagsForm
//{
//    public class AssignFormTag(IMongoRepository<Form> _mongoRepository, GetByIdForm _getById)
//    {
//        public async Task<Result<CustomTag>> Execute(string formId, ActionFormTagRequest request)
//        {
//            return await _getById.Execute(formId)
//                .Bind(form => ValidateUniqueName(form, request))
//                .Bind(form => AddFormTag(form, request));
//        }

//        public static Result<Form> ValidateUniqueName(Form form, ActionFormTagRequest request)
//        {
//            bool tagExists = form.Tags.Any(t =>
//                t.IsActive &&
//                t.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
//            );

//            if (tagExists)
//                return Result.Failure<Form>("A tag with the same name already exists in this form");

//            return form.Success();
//        }

//        public async Task<Result<CustomTag>> AddFormTag(Form form, ActionFormTagRequest request)
//        {
//            var newTag = request.ToCustomTag();

//            form.Tags.Add(newTag);

//            await _mongoRepository.ReplaceOneAsync(form);

//            return newTag.Success(HttpStatusCode.OK);
//        }
//    }
//}