using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgoraCertaminaBack.Models.Entities
{
    public class Tag : IEntity
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("color"), BsonRepresentation(BsonType.String)]
        public required string Color { get; set; }

        [BsonElement("category"), BsonRepresentation(BsonType.String)]
        public TagCategory Category { get; set; }

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
