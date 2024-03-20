using System.Text.Json.Serialization;

namespace MyTaskManager.Models.DTO.TaskDTO
{
    public class UpdateTaskDTO
    {
        public string? Title { get; set; } = string.Empty;
        public string? CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityFrom Priority { get; set; }
        public DateTime Expiration { get; set; }
    }
}
