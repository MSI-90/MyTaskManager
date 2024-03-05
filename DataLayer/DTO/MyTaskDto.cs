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
        public string TitleTask { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;

        [JsonIgnore]
        public PriorityFrom Prior { get; set; }
        public string PriorityString {  
            get {  return Prior.ToString(); }
            set { Prior = (PriorityFrom)Enum.Parse(typeof(PriorityFrom), value); }
        }
        public DateTime Expiration { get; set; } = DateTime.Now;
        public string PriorityDescription
        { 
            get { return new TaskService().GetPriorityDescription(Prior); } 
        }
    }
}