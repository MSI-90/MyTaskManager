namespace MyTaskManager.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
