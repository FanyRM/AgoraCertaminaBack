using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Tag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Customers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tags
{
    public class CreateTag(IMongoRepository<Customer> _mongoRepository, GetByIdCustomer _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CustomTagRequest request)
        {
            var result = await _getById.Execute(_userRequest.CustomerId)
                .Bind(customer => ValidateUniqueTagName(customer, request.Name))
                .Bind(customer => AddCustomerTag(customer, request));

            return result;
        }

        private static Result<Customer> ValidateUniqueTagName(Customer customer, string tagName)
        {
            bool tagExists = customer.Tags.Any(t =>
                t.IsActive &&
                t.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase)
            );

            if (tagExists)
                return Result.Failure<Customer>("A tag with this name already exists");

            return customer.Success();
        }

        public async Task<Result<string>> AddCustomerTag(Customer customer, CustomTagRequest request)
        {
            var newTag = request.CustomTagRequestToCustomTag();

            customer.Tags.Add(newTag);

            await _mongoRepository.ReplaceOneAsync(customer);

            return newTag.Id.Success(HttpStatusCode.OK);
        }
    }
}