using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.Data
{
    public class Priority
    {
        [JsonIgnore]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(100)]
        public string Descrption { get; set; } = String.Empty;

        [JsonIgnore]
        public ICollection<MyTask>? Tasks { get; set; }
    }
}
