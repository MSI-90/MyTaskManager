using System.ComponentModel.DataAnnotations;

namespace Models.EfClasses
{
    public class Priority
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = String.Empty;
        public ICollection<MyTask>? Tasks { get; set; }
    }
}
