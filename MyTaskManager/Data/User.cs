using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Data
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string FirstName { get; set; } = String.Empty;

        [MaxLength(30)]
        public string LastName { get; set; } = String.Empty;
        public ICollection<MyTask> Tasks { get; set; }
    }
}
