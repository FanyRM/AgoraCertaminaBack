using AgoraCertaminaBack.Models.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgoraCertaminaBack.Models.Entities
{
    public class Form : IEntity, ICustomerAttribute
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("tenant_id"), BsonRepresentation(BsonType.ObjectId)]
        public required string OrganizationId { get; set; }

        [BsonElement("form_name"), BsonRepresentation(BsonType.String)]
        public required string FormName { get; set; }

        [BsonElement("tenant_name"), BsonRepresentation(BsonType.String)]
        public required string TenantName { get; set; }

        [BsonElement("form_fields")]
        public List<CustomField> FormFields { get; set; } = new List<CustomField>();

        [BsonElement("tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
