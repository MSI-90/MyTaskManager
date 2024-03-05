using System.Text.Json.Serialization;

namespace DataLayer.DTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityFrom : byte
    {
        Низкий,
        Нормальный,
        Высокий
    }
}