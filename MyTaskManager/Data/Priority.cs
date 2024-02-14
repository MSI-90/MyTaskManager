namespace MyTaskManager.Data
{
    public class Priority
    {
        public string? Name { get; set; }
        public string? Descrption { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
