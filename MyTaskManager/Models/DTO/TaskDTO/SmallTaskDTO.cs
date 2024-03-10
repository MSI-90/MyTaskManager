namespace MyTaskManager.Models.DTO.TaskDTO
{
    public class SmallTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
