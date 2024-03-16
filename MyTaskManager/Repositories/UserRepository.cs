﻿using Microsoft.IdentityModel.Tokens;
using Models.EfClasses;
using MyTaskManager.EfCode;
using MyTaskManager.Infrastructure;
using MyTaskManager.Models;
using MyTaskManager.Models.DTO.UserDTO;
using MyTaskManager.Models.DTO.UserDTO.AuthDTO;
using MyTaskManager.Models.DTO.UserDTO.RegistrationDTO;
using MyTaskManager.Models.UserDTO.AuthDTO;
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
        PasswordHasher passwordHasher;
        public UserRepository(TaskContext taskContext, IConfiguration configuration)
        {
            _taskContext = taskContext;
            _secretKey = configuration.GetValue<string>("JWT:Secret", _secretKey) ?? _secretKey;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _taskContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            passwordHasher = new PasswordHasher();

            var user = _taskContext.Users.AsEnumerable().FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() &&
            passwordHasher.Verify(loginRequestDTO.Password, u.Password));

            if (user == null)
                return new LoginResponseDTO
                {
                    Token = "",
                    User = null
                };

            LocalUser localUserDTO = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var authJwtClaims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.UserName),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authJwtClaims),
                Expires = DateTime.Now.AddMinutes(1)/*AddDays(3)*/,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = localUserDTO
            };

            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {

            var passwordHasher = new PasswordHasher();
            LocalUser localUser = new()
            {
                UserName = registerationRequestDTO.UserName,
                LastName = registerationRequestDTO.LastName,
                FirstName = registerationRequestDTO.FirstName,
                Email = registerationRequestDTO.Email,
                Password = passwordHasher.Generate(registerationRequestDTO.Password),
                Role = UserRoles.User.ToString()
            };

            User user = new()
            {
                UserName = localUser.UserName,
                LastName = localUser.LastName,
                FirstName = localUser.FirstName,
                Email = localUser.Email,
                Password = localUser.Password,
                Role = UserRoles.User.ToString()
            };

            _taskContext.Users.Add(user);
            await _taskContext.SaveChangesAsync();

            localUser.Password = "";
            return localUser;
        }
    }
}