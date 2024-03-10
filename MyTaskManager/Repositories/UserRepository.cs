using Models.EfClasses;
using MyTaskManager.EfCode;
using MyTaskManager.Models.DTO.User.AuthDTO;
using MyTaskManager.Models.DTO.User.RegistrationDTO;
using MyTaskManager.Models.DTO.UserDTO;
using MyTaskManager.Repositories.Interfaces;

namespace MyTaskManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskContext _taskContext;
        public UserRepository(TaskContext taskContext) => _taskContext = taskContext;
        public bool IsUniqueUser(string username)
        {
            var user = _taskContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
                return true;

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            User user = new()
            {
                UserName = registerationRequestDTO.UserName,
                LastName = registerationRequestDTO.LastName,
                FirstName = registerationRequestDTO.FirstName,
                Email = registerationRequestDTO.Email,
                Password = registerationRequestDTO.Password,
                Role = UserRoles.User.ToString()
            };

            _taskContext.Users.Add(user);
            await _taskContext.SaveChangesAsync();

            user.Password = "";
            return user;
        }
    }
}
