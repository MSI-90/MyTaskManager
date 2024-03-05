using DataLayer.DTO;
using ServiceLayer.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.DTO
{
    public class MyTaskDto
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string TitleTask { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityFrom Prior { get; set; }
        public DateTime Expiration { get; set; } = DateTime.Now;

        [JsonIgnore]
        public string PriorityDescription
        { 
            get { return new TaskService().GetPriorityDescription(Prior); }
        }
    }
}