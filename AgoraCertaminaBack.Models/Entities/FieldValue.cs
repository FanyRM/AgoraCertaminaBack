using AgoraCertaminaBack.Models.Entities.Interfaces;
using AgoraCertaminaBack.Models.General;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Entities
{
    public class FieldValue : IEntity
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("field_name"), BsonRepresentation(BsonType.String)]
        public required string FieldName { get; set; }

        [BsonElement("value")]
        public BsonValue? Value { get; set; }

        [BsonElement("field_type"), BsonRepresentation(BsonType.String)]
        public FieldTypeEnum Type { get; set; }

        [BsonElement("catalog_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? CatalogId { get; set; }

        [BsonElement("is_base"), BsonRepresentation(BsonType.Boolean)]
        public bool IsBase { get; set; }

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
