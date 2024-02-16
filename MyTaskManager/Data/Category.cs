using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Data
{
    public class Category
    {
        public int Id { get; set; }


        [MaxLength(20)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(50)]
        public string Description { get; set; } = String.Empty;
        public ICollection<MyTask> Tasks { get; set; }
    }
}
