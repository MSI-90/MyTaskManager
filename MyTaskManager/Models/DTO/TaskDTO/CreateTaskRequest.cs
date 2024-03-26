
using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Models.DTO.TaskDTO
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Название задачи может содержать не более 100 символов")]
        public string TitleTask { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; } = string.Empty;
        public string Prior { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime Expiration { get; set; } = DateTime.Now;
    }
}
