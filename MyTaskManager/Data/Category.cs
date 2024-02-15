using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
