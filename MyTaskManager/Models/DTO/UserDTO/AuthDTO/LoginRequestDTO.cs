using System.ComponentModel.DataAnnotations;

namespace MyTaskManager.Models.DTO.UserDTO.AuthDTO
{
    public class LoginRequestDTO
    {
        public string UserName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
