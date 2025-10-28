using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AgoraCertaminaBack.Models.Entities.Interfaces;

namespace AgoraCertaminaBack.Models.Entities
{
    public class Participant : IEntity, ICustomerAttribute
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("tenant_id"), BsonRepresentation(BsonType.ObjectId)]
        public string OrganizationId { get; set; } = string.Empty;

        [BsonElement("tenant_name"), BsonRepresentation(BsonType.String)]
        public required string TenantName { get; set; }

        [BsonElement("external_id"), BsonRepresentation(BsonType.String)]
        public string? ExternalId { get; set; }

        [BsonElement("first_name"), BsonRepresentation(BsonType.String)]
        public required string FirstName { get; set; }

        [BsonElement("last_name"), BsonRepresentation(BsonType.String)]
        public required string LastName { get; set; }

        [BsonElement("phone_number"), BsonRepresentation(BsonType.String)]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string Email { get; set; } = string.Empty;

        [BsonElement("tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
