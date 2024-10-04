using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
	[ApiController]
	[Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiVersionNeutral]
	public class UsersController : Controller
	{
		private readonly IUserRepository _userRepository;
        protected APIResponse _response; 

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;   
            this._response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepository.Login(loginRequestDTO);

            if(loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("UserName or Password is incorrect");
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode=HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDTO registrationRequest)
        {
            var isValid = _userRepository.IsUniqueUser(registrationRequest.UserName);

            if(!isValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("The userName already exists");
                return BadRequest(_response);
            }

            var user = await _userRepository.Register(registrationRequest);

            if (user.UserName == null || user.Name == null || user.ID == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(user);
        }
    }
}
