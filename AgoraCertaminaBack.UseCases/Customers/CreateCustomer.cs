using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers.CustomerMap;

using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Customers
{
    public class CreateCustomer(IMongoRepository<Customer> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string name)
        {
            var result = await ValidateUniqueCustomerName(name)
                .Bind(CreateNewCustomer);

            return result;
        }

        private async Task<Result<string>> ValidateUniqueCustomerName(string name)
        {
            bool customerExists = await _mongoRepository.ExistsAsync(c =>
                c.IsActive &&
                c.CustomerName.Equals(name, StringComparison.CurrentCultureIgnoreCase)
            );

            if (customerExists)
                return Result.Failure<string>("A customer with this name already exists");

            return name.Success();
        }

        private async Task<Result<string>> CreateNewCustomer(string name)
        {
            var customer = name.ToCustomer();

            await _mongoRepository.InsertOneAsync(customer);

            return customer.Id.Success(HttpStatusCode.Created);
        }
    }
}
