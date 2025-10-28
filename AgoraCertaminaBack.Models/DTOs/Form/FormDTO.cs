using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;

namespace AgoraCertaminaBack.Models.DTOs.Form
{
    public class FormProgrammedDTO
    {
        public required string Id { get; set; }
        public required string FormId { get; set; }
        public required string FormName { get; set; }
        public required string Folio { get; set; }
        public DateTime LaunchDate { get; set; }
        public DateTime CloseDate { get; set; }
        public LaunchType LaunchType { get; set; }
    }

    public class FormAssignedDTO
    {
        public required string Id { get; set; }
        public required string FormId { get; set; }
        public required string FormName { get; set; }
        public string? ParticipantId { get; set; }
        public string? ParticipantName { get; set; }
        public FormStatus FormStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime CloseDate { get; set; }
    }

    public class FormDTO
    {
        public required string Id { get; set; }
        public required string OrganizationId { get; set; }
        public required string FormName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CountSchemaFields { get; set; }
        public List<CustomTagDTO> Tags { get; set; } = [];
        public List<CustomFieldDTO> FormFields { get; set; } = [];
        public LaunchType? LaunchType { get; set; }
    }

    public class FormPreviewDTO
    {
        public string FormProgrammedId { get; set; } = string.Empty;
        public string FormProgrammedFolio { get; set; } = string.Empty;
        public string FormId { get; set; } = string.Empty;
        public string ResponseId { get; set; } = string.Empty;
        public string FormName { get; set; } = string.Empty;
        public DateTime LaunchDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string? FormAssignedId { get; set; }
        public FormStatus? FormStatus { get; set; }
        public bool RequiresAccessKey { get; set; } = true;
    }

    public class UpdateFormAssignedRequest
    {
        public required string FormAssignedId { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
