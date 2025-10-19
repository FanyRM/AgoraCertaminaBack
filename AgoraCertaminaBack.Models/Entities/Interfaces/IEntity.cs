namespace AgoraCertaminaBack.Models.Entities.Interfaces
{
    public interface IEntity
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
