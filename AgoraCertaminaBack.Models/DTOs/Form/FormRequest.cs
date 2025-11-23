using AgoraCertaminaBack.Models.DTOs.Form.FormTag;

namespace AgoraCertaminaBack.Models.DTOs.Form
{
    public class CreateFormRequest
    {
        public required string FormName { get; set; }
        public List<ActionFormTagRequest> Tags { get; set; } = new List<ActionFormTagRequest>();
    }


    public class UpdateFormRequest
    {
        public required string FormName { get; set; }
        public required List<ActionFormTagRequest> Tags { get; set; }
    }


}
