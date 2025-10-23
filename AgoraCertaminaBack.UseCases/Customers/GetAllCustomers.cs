using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Customer;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.Customers
{
    public class GetAllCustomers(IMongoRepository<Customer> _mongoRepository)
    {
        public async Task<Result<List<CustomerSummaryDTO>>> Execute()
        {
            var customers = await _mongoRepository.FilterByAsync(customer => customer.IsActive == true);

            var customerSummaries = customers.Select(customer => new CustomerSummaryDTO
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                CatalogsCount = customer.Catalogs.Count(c => c.IsActive),
                CreatedAt = customer.CreatedAt,
                IsActive = customer.IsActive
            }).ToList();

            return customerSummaries.Success();
        }
    }
}