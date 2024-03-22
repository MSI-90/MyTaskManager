using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Models.DTO.TaskDTO
{
    public class UpdateTaskDTO
    {
        [MaxLength(100, ErrorMessage = "Название задачи может составлять не более 100 символов")]
        public string? Title { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Наименование категории не должно превышать 50 символов")]
        public string? CategoryName { get; set; } = string.Empty;

        [MaxLength(150, ErrorMessage = "Описание категории не должно превышать 150 символов")]
        public string? CategoryDescription { get; set; } = string.Empty;
        public PriorityFrom Priority { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Expiration { get; set; } = DateTime.Now;
    }
}
