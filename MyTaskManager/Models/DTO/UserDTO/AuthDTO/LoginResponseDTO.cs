namespace MyTaskManager.Models.UserDTO.AuthDTO
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
