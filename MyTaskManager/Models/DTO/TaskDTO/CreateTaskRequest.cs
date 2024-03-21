
using Helpers;
using Models.EfClasses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.Models.DTO.TaskDTO
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Название задачи может содержать не более 100 символов")]
        public string TitleTask { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; } = string.Empty;
        public PriorityFrom Prior { get; set; }
        public DateTime Expiration { get; set; } = DateTime.Now;
    }
}
