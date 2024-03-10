using Microsoft.IdentityModel.Tokens;
using Models.EfClasses;
using MyTaskManager.EfCode;
using MyTaskManager.Models.DTO.User.AuthDTO;
using MyTaskManager.Models.DTO.User.RegistrationDTO;
using MyTaskManager.Models.DTO.UserDTO;
using MyTaskManager.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTaskManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskContext _taskContext;
        private readonly string _secretKey = "";
        public UserRepository(TaskContext taskContext, IConfiguration configuration)
        {
            _taskContext = taskContext;
            _secretKey = configuration.GetValue<string>("JWT:Secret", _secretKey);
        }
        public bool IsUniqueUser(string username)
        {
            var user = _taskContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
                return true;

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _taskContext.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() &&
            u.Password == loginRequestDTO.Password);

            if (user == null)
                return null;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var authJwtClaims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.UserName),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authJwtClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            authJwtClaims.Add(new Claim(ClaimTypes.Role, user.Role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authJwtClaims),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return loginResponseDTO;
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