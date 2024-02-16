using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Data
{
    public class Priority
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(50)]
        public string Descrption { get; set; } = String.Empty;
        public ICollection<Task> Tasks { get; set; }
    }
}
