using AgoraCertaminaBack.Models.Entities.Interfaces;
using AgoraCertaminaBack.Models.General;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgoraCertaminaBack.Models.Entities
{
    public class FormResponse : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("tenant_id")]
        public string TenantId { get; set; } = string.Empty;

        [BsonElement("tenant_name")]
        public string TenantName { get; set; } = string.Empty;

        [BsonElement("form_id")]
        public string FormId { get; set; } = string.Empty;

        [BsonElement("form_name")]
        public string FormName { get; set; } = string.Empty;

        [BsonElement("contest_id")]
        public string? ContestId { get; set; }

        [BsonElement("participant_id")]
        public string ParticipantId { get; set; } = string.Empty;

        [BsonElement("participant_name")]
        public string ParticipantName { get; set; } = string.Empty;

        [BsonElement("field_responses")]
        public List<FieldResponse> FieldResponses { get; set; } = new();

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("submitted_at")]
        public DateTime? SubmittedAt { get; set; }

        [BsonElement("is_active")]
        public bool IsActive { get; set; }
    }

    public class FieldResponse
    {
        public string FieldId { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public FieldTypeEnum Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public string? CatalogId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public enum FormResponseStatus
    {
        Draft = 0,
        Submitted = 1,
        InReview = 2,
        Approved = 3,
        Rejected = 4
    }
}
