using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class TenantMappers
    {
        public static Tenant ToTenant(this string name)
        {
            return new Tenant
            {
                Id = ObjectId.GenerateNewId().ToString(),
                TenantName = name,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
