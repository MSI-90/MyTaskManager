using System.Text.Json.Serialization;

namespace MyTaskManager.Models.DTO.TaskDTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityFrom : byte
    {
        Низкий,
        Нормальный,
        Высокий
    }
}