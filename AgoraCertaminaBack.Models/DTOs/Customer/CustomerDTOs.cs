namespace AgoraCertaminaBack.Models.DTOs.Customer
{
    public class CustomerSummaryDTO
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int CatalogsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerNameResponse
    {
        public string CustomerName { get; set; } = string.Empty;
    }

}
