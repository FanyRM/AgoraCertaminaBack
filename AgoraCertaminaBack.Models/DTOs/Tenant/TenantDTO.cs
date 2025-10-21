namespace AgoraCertaminaBack.Models.DTOs.Tenant
{
    public class TenantDTO
    {
        public string Id { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public int CatalogsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
