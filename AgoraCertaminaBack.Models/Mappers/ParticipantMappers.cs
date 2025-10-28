using MongoDB.Bson;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Participant;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class ParticipantMappers
    {
        #region Participant Creation Mapping
        public static Participant ToParticipant( this CreateParticipantRequest createRequest, string customerId, string customerName)
        {
            return new Participant
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrganizationId = customerId,
                TenantName = customerName,
                FirstName = createRequest.FirstName,
                LastName = createRequest.LastName,
                PhoneNumber = createRequest.PhoneNumber,
                Email = createRequest.Email,
                Tags = createRequest.Tags?.Select(tag => new Tag
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = tag.Color
                }).ToList() ?? [],
                CreatedAt = DateTime.Now,
                IsActive = true
            };
        }

        #endregion

        #region Participant DTO Mappings

        public static ParticipantDTO ToParticipantDTO(this Participant participant)
        {
            return new ParticipantDTO
            {
                Id = participant.Id,
                ExternalId = participant.ExternalId ?? "",
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                PhoneNumber = participant.PhoneNumber,
                Email = participant.Email,
                Tags = participant.Tags?.Select(tag => tag.ToCustomTagDTO()).ToList() ?? new List<CustomTagDTO>(),
                IsActive = participant.IsActive,
                CreatedAt = participant.CreatedAt
            };
        }

        public static BasicParticipantDTO ToBasicParticipantDTO(this Participant participant)
        {
            return new BasicParticipantDTO
            {
                Id = participant.Id,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                PhoneNumber = participant.PhoneNumber,
                Email = participant.Email
            };
        }

        #endregion
    }
}