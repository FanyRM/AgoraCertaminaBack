using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Tenant;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tenants
{
    public class CreateTenant(IMongoRepository<Tenant> _mongoRepository)
    {
        public async Task<Result<TenantDTO>> Execute(string name)
        {
            return await ValidateUniqueName(name)
                .Bind(CreateNewTenant);
        }

        private async Task<Result<string>> ValidateUniqueName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<string>("El nombre del tenant no puede estar vacío");
            }

            bool exists = await _mongoRepository.ExistsAsync(c =>
                c.IsActive &&
                c.TenantName.Equals(name, StringComparison.CurrentCultureIgnoreCase)
            );

            if (exists)
                return Result.Failure<string>("A tenant with this name already exists");

            return name.Success();
        }

        private async Task<Result<TenantDTO>> CreateNewTenant(string name)
        {
            var tenant = name.ToTenant();
            await _mongoRepository.InsertOneAsync(tenant);

            var tenantDTO = new TenantDTO
            {
                Id = tenant.Id,
                TenantName = tenant.TenantName,
                CreatedAt = tenant.CreatedAt,
                IsActive = tenant.IsActive
            };

            return tenantDTO.Success(HttpStatusCode.Created);
        }
    }
}