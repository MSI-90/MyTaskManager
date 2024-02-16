using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.Data
{
    public class Category
    {
        [JsonIgnore]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(100)]
        public string Description { get; set; } = String.Empty;

        [JsonIgnore]
        public ICollection<MyTask> Tasks { get; set; }
    }
}
