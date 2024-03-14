using Microsoft.AspNetCore.Mvc;
using System.Net;
using MyTaskManager.Models;
using MyTaskManager.Repositories.Interfaces;
using MyTaskManager.Models.DTO.UserDTO.AuthDTO;
using MyTaskManager.Models.DTO.UserDTO.RegistrationDTO;
using Microsoft.Extensions.Localization;

namespace MyTaskManager.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IStringLocalizer<UserController> _localizer;
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;
        public UserController(IStringLocalizer<UserController> localizer, IUserRepository userRepo)
        {
            _localizer = localizer;
            _userRepo = userRepo;
            this._response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                string errorMessage = _localizer["Псевдоним или пароль указаны неверно"];
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(errorMessage);
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Пользователь уже существует");
                return BadRequest(_response);
            }

            var userReg = await _userRepo.Register(model);
            if (userReg == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Ошибка во время регистрации");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = userReg;
            return Ok(_response);
        }
    }
}
