using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgoraCertaminaBack.Models.Entities
{
    public class SchemaContest : IEntity, ICustomerAttribute
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("organization_id"), BsonRepresentation(BsonType.ObjectId)]
        public required string OrganizationId { get; set; }

        [BsonElement("schema_name"), BsonRepresentation(BsonType.String)]
        public required string SchemaName { get; set; }

        [BsonElement("customer_name"), BsonRepresentation(BsonType.String)]
        public required string CustomerName { get; set; }

        [BsonElement("schema_fields")]
        public List<Field> SchemaFields { get; set; } = new List<Field>();

        [BsonElement("tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
