using Models.EfClasses;
using MyTaskManager.Models.DTO.User.AuthDTO;
using MyTaskManager.Models.DTO.User.RegistrationDTO;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<User> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
