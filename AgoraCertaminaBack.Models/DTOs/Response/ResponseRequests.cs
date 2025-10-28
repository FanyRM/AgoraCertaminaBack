using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.DTOs.Participant;

namespace AgoraCertaminaBack.Models.DTOs.Response
{
    public class AssignedFormResponseRequest
    {
        public required string FormProgrammedId { get; set; }
        public required string FormAssignedId { get; set; }
        public required CreateResponseRequest CreateResponse { get; set; }
    }

    public class AnonymousFormResponseRequest
    {
        public required string FormProgrammedId { get; set; }
        public required CreateResponseRequest CreateResponse { get; set; }
    }

    public class OpenFormResponseRequest
    {
        public required string FormProgrammedId { get; set; }
        public required CreateParticipantRequest Participant { get; set; }
        public required CreateResponseRequest CreateResponse { get; set; }
    }

    public class CreateResponseRequest
    {
        public required string FormId { get; set; }
        public required string FormName { get; set; }
        public List<FieldDTO> Fields { get; set; } = [];
    }

    public class UpdateResponseRequest
    {
        //public List<TagDTO>? Tags { get; set; }
        public List<FieldDTO>? Fields { get; set; }
        public string? FeedbackKey { get; set; }
    }

    public class DeleteResponseRequest
    {
        public required string AssetId { get; set; } = null!;
    }
}
