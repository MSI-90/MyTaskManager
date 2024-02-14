namespace MyTaskManager.Data
{
    public class Task
    {
        public int Id { get; set; }
        public string? TitleTask { get; set; }
        public Category? CategoryId { get; set; }
        public Priority? PrioryName { get; set; }
        public DateTime Expiration { get; set; }
        public User? UserId { get; set; }
    }
}
