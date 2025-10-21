using AgoraCertaminaBack.Models.DTOs.CustomTag;

namespace AgoraCertaminaBack.Models.DTOs.Participant
{
    public class CreateParticipantRequest
    {
        public string? ExternalId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<CustomTagDTO> Tags { get; set; } = [];
    }

    public class UpdateParticipantRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
