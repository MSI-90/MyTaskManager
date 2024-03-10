using System.Text.Json.Serialization;

namespace MyTaskManager.Models.DTO.UserDTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoles : byte
    {
        Guest = 1,
        User,
        Admin
    }
}
