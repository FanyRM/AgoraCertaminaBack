
using AgoraCertaminaBack.Models.DTOs.CustomTag;

namespace AgoraCertaminaBack.Models.DTOs.Participant
{
    public class ParticipantDTO
    {
        public required string Id { get; set; }
        public required string ExternalId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<CustomTagDTO> Tags { get; set; } = [];
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BasicParticipantDTO
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}