using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;

namespace AgoraCertaminaBack.Models.DTOs.Form
{

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

}
