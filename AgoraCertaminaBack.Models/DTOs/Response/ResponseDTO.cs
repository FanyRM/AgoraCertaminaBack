using AgoraCertaminaBack.Models.DTOs.Participant;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.Response
{
    public class ResponseDTO
    {
        public required string Id { get; set; }
        public required string FormId { get; set; }
        public required string FormName { get; set; }
        public required string FormProgrammedId { get; set; }
        public required string FormProgrammedFolio { get; set; }
        public required string ParticipantName { get; set; }
        public List<ResponseTagsDTO> Tags { get; set; } = new();
        public List<FieldDTO> Fields { get; set; } = new();
        public BasicParticipantDTO? AssignedTo { get; set; }
        public string? AssignedId { get; set; }
    }

    public class ResponseTagsDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
        public required string Subcategory { get; set; }
    }

    public class FieldDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public string? CatalogId { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
