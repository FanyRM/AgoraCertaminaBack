using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.Customers
{
    public class GetByIdCustomer(IMongoRepository<Customer> _mongoRepository)
    {
        public async Task<Result<Customer>> Execute(string customerId)
        {
            var customer = await _mongoRepository.FindByIdAsync(customerId);

            if (customer == null)
                return Result.NotFound<Customer>("Customer not found");

            return customer;
        }
    }
}
