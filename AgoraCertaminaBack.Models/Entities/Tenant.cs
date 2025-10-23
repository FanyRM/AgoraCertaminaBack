using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgoraCertaminaBack.Models.Entities
{
    public class Tenant : IEntity
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("tenant_name"), BsonRepresentation(BsonType.String)]
        public required string TenantName { get; set; }

        [BsonElement("tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BsonElement("catalogs")]
        public List<Catalog> Catalogs { get; set; } = new List<Catalog>();

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
