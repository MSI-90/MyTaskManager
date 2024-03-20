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
        private readonly IStringLocalizer<UserController> _stringLocalizer;

        private readonly IUserRepository _userRepo;
        protected APIResponse _response;
        public UserController(IStringLocalizer<UserController> stringLocalizer, IUserRepository userRepo)
        {
            _stringLocalizer = stringLocalizer;
            _userRepo = userRepo;
            this._response = new();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromForm] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                var stringFromResources = _stringLocalizer["UsernameOrPasswordIsIncorrect"].Value;

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(stringFromResources);
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromForm] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (ifUserNameUnique)
            {
                var stringFromResources = _stringLocalizer["UserIsAlreadyExist"].Value;

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(stringFromResources);
                return BadRequest(_response);
            }

            var userReg = await _userRepo.Register(model);
            if (userReg == null)
            {
                var stringFromResources = _stringLocalizer["ErrorDuringRegistration"].Value;

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(stringFromResources);
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = userReg;
            return Ok(_response);
        }
    }
}
