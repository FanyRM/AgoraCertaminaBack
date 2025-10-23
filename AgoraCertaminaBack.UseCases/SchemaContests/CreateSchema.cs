using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Customers;
using AgoraCertaminaBack.Models.Mappers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class CreateSchema(IMongoRepository<SchemaContest> _mongoRepository, GetByIdCustomer _getByIdCustomer, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CreateSchemaContestRequest request)
        {
            var result = await _getByIdCustomer.Execute(_userRequest.CustomerId)
                .Bind(customer => ValidateUniqueSchemaName(customer, request))
                .Bind(customer => CreateNewSchema(customer, request));

            return result;
        }

        private async Task<Result<Customer>> ValidateUniqueSchemaName(Customer customer, CreateSchemaContestRequest request)
        {
            bool schemaExists = await _mongoRepository.ExistsAsync(s =>
                s.IsActive &&
                s.CustomerId == customer.Id &&
                s.SchemaName == request.SchemaName
            );

            if (schemaExists)
                return Result.Failure<Customer>("A schema with this name already exists");

            return customer.Success();
        }

        private async Task<Result<string>> CreateNewSchema(Customer customer, CreateSchemaContestRequest request)
        {
            var tags = request.Tags.Select(t => t.ToCustomTag()).ToList();

            var newSchema = new SchemaContest
            {
                CustomerId = customer.Id,
                CustomerName = customer.CustomerName,
                SchemaName = request.SchemaName,
                Tags = tags,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _mongoRepository.InsertOneAsync(newSchema);

            return newSchema.Id.Success(HttpStatusCode.Created);
        }
    }
}