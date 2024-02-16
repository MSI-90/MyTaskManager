using MyTaskManager.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.DTO
{
    public class MyTaskDto
    {
        //[JsonIgnore]
        public int Id { get; set; }

        [MaxLength(50)]
        public string TitleTask { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public string Priory { get; set; } = string.Empty;
        public string PriorityDescription {  get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string UserLastName { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
    }
}
