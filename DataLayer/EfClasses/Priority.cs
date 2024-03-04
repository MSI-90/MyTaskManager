using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.Data
{
    public class Priority
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = String.Empty;
        public ICollection<MyTask>? Tasks { get; set; }
    }
}
