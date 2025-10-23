using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Mappers.CustomerMap
{
    public static class CustomerMappers
    {
        public static Entities.Customer ToCustomer(this string name)
        {
            return new Entities.Customer
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CustomerName = name,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
