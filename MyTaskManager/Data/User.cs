using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.Data
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }

        [MaxLength(20)]
        public string FirstName { get; set; } = String.Empty;

        [MaxLength(30)]
        public string LastName { get; set; } = String.Empty;

        [JsonIgnore]
        public ICollection<MyTask> Tasks { get; set; }
    }
}
