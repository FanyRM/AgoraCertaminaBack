using AgoraCertaminaBack.Models.DTOs.Form.FormTag;

namespace AgoraCertaminaBack.Models.DTOs.Form
{
    public class CreateFormProgrammedRequest
    {
        public required string FormId { get; set; }
        public LaunchType LaunchType { get; set; }
        public DateTime LaunchDate { get; set; }
        public DateTime CloseDate { get; set; }
    }

    public class CreateFormAssignmentRequest
    {
        public required string FormProgrammedId { get; set; }
        public required string ParticipantId { get; set; }
    }

    public class CreateFormRequest
    {
        public required string FormName { get; set; }
        public List<ActionFormTagRequest> Tags { get; set; } = new List<ActionFormTagRequest>();
    }

    public class UpdateFormProgrammedRequest
    {
        public required string FormProgrammedId { get; set; }
        public DateTime LaunchDate { get; set; }
        public DateTime CloseDate { get; set; }
    }

    public class UpdateFormRequest
    {
        public required string FormName { get; set; }
        public required List<ActionFormTagRequest> Tags { get; set; }
    }

    public class AccessKeyRequest
    {
        public string AccessKey { get; set; } = string.Empty;
    }

}
