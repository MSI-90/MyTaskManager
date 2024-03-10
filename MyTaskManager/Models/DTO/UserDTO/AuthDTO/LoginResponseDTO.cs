using Models.EfClasses;

namespace MyTaskManager.Models.DTO.User.AuthDTO
{
    public class LoginResponseDTO
    {
        public User User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
