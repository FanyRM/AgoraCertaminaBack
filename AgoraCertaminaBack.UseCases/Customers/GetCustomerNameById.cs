using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Customer;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.Customers
{
    public class GetCustomerNameById(IMongoRepository<Customer> _mongoRepository)
    {
        public async Task<Result<CustomerNameResponse>> Execute(string customerId)
        {
            var customer = await _mongoRepository.FindByIdAsync(customerId);

            if (customer == null)
                return Result.NotFound<CustomerNameResponse>("Customer not found");

            return new CustomerNameResponse
            {
                CustomerName = customer.CustomerName
            };
        }
    }
}
