namespace MyTaskManager.Data
{
    public class Task
    {
        public int Id { get; set; }
        public string TitleTask { get; set; } = string.Empty;
        public Category Category { get; set; }
        public Priority Priory { get; set; }
        public DateTime Expiration { get; set; }
        public User? User { get; set; }
    }
}
