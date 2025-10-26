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

        [BsonElement("organization_id"), BsonRepresentation(BsonType.ObjectId)]
        public string OrganizationId { get; set; } = null!;

        [BsonElement("schema_id"), BsonRepresentation(BsonType.ObjectId)]
        public string SchemaId { get; set; } = null!;

        [BsonElement("form_id"), BsonRepresentation(BsonType.ObjectId)]
        public string FormId { get; set; } = null!;

        [BsonElement("reference_number"), BsonRepresentation(BsonType.String)]
        public string ReferenceNumber { get; set; } = null!;

        [BsonElement("organization_name"), BsonRepresentation(BsonType.String)]
        public string OrganizationName { get; set; } = null!;

        [BsonElement("schema_name"), BsonRepresentation(BsonType.String)]
        public string SchemaName { get; set; } = null!;

        [BsonElement("contest_name"), BsonRepresentation(BsonType.String)]
        public string ContestName { get; set; } = null!;

        [BsonElement("description_contest"), BsonRepresentation(BsonType.String)]
        public string DescriptionContest { get; set; } = null!;

        [BsonElement("image_url"), BsonRepresentation(BsonType.String)]
        public string ImageUrl { get; set; } = null!;

        [BsonElement("start_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime StartDate { get; set; }

        [BsonElement("end_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime EndDate { get; set; }

        [BsonElement("tags")]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BsonElement("fields")]
        public List<FieldValue> Fields { get; set; } = new List<FieldValue>();

        [BsonElement("is_evalued"), BsonRepresentation(BsonType.Boolean)]
        public bool IsEvalued { get; set; } = false;

        [BsonElement("is_suspended"), BsonRepresentation(BsonType.Boolean)]
        public bool IsSuspended { get; set; } = false;

        [BsonElement("is_pay"), BsonRepresentation(BsonType.Boolean)]
        public bool IsPay { get; set; } = false;

        [BsonElement("price"), BsonRepresentation(BsonType.Double)]
        public double? Price { get; set; }

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("is_active"), BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; } = true;
    }
}
