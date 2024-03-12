using System.Text.Json.Serialization;

namespace MyTaskManager.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityFrom : byte
    {
        Низкий,
        Нормальный,
        Высокий
    }
}