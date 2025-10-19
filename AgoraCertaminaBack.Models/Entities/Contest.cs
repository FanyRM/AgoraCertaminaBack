using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.Entities.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Entities
{
    public class Contest : IEntity, ICustomerAttribute
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("customer_id"), BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = null!;

        [BsonElement("schema_id"), BsonRepresentation(BsonType.ObjectId)]
        public string SchemaId { get; set; } = null!;

        [BsonElement("reference_number"), BsonRepresentation(BsonType.String)]
        public string ReferenceNumber { get; set; } = null!;

        [BsonElement("customer_name"), BsonRepresentation(BsonType.String)]
        public string CustomerName { get; set; } = null!;

        [BsonElement("schema_name"), BsonRepresentation(BsonType.String)]
        public string SchemaName { get; set; } = null!;

        [BsonElement("tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BsonElement("fields")]
        public List<FieldValue> Fields { get; set; } = new List<FieldValue>();

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
