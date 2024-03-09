using System.Text.Json.Serialization;

namespace Models.DTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityFrom : byte
    {
        Низкий,
        Нормальный,
        Высокий
    }
}