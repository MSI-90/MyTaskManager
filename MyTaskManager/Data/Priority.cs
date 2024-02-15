using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Data
{
    public class Priority
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Descrption { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
