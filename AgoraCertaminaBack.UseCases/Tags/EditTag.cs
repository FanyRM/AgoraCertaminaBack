using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Tag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Customers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tags
{
    public class EditTag(IMongoRepository<Customer> _mongoRepository, GetByIdCustomer _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<Tag>> Execute(EditCustomTagRequest request)
        {
            return await _getById.Execute(_userRequest.CustomerId)
                .Bind(customer => AddCustomTagChanged(customer, request));
        }

        public async Task<Result<Tag>> AddCustomTagChanged(Customer customer, EditCustomTagRequest request)
        {
            var tagToUpdate = customer.Tags.Find(tag => tag.Id == request.Id);

            if (tagToUpdate == null)
                return Result.NotFound<Tag>("Tag not found");

            tagToUpdate.Name = request.Name;
            tagToUpdate.Color = request.Color;

            await _mongoRepository.ReplaceOneAsync(customer);

            return tagToUpdate.Success(HttpStatusCode.OK);

        }
    }
}
