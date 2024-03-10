using MyTaskManager.Models;
using MyTaskManager.Models.DTO.UserDTO.AuthDTO;
using MyTaskManager.Models.DTO.UserDTO.RegistrationDTO;
using MyTaskManager.Models.UserDTO.AuthDTO;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
