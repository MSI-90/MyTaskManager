using System.ComponentModel.DataAnnotations;

namespace Models.EfClasses
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Description { get; set; } = string.Empty;
        public ICollection<MyTask> Tasks { get; set; }
        public ICollection<User> Users { get; set; }
    }
}