using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;
using OxideWebApi.Services;
using OxideClassLibrary.Extensions;

namespace OxideWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ILoginService service) : ControllerBase
    {
        private readonly ILoginService _service = service;

        [AllowAnonymous]
        [HttpGet("IsLoginAvailable/{newLogin}")]
        public async Task<ActionResult<string>> CheckLogin(string newLogin)
        {
            var isLoginAvailable = await _service.CheckLoginAsync(newLogin);

            return isLoginAvailable
                ? Ok(new
                {
                    Message = "Логин свободен",
                    Date = DateTime.UtcNow
                })
                : BadRequest(new
                {
                    Message = "Логин уже занят другим пользователем",
                    StatusCode = "BadRequest [400]",
                    Date = DateTime.UtcNow,
                });
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public async Task<ActionResult<string>> AuthUser(LoginRequest loginData)
        {
            var result = await _service.AuthUserAsync(loginData);

            return result is null
                ? Unauthorized()
                : result;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<UserData>> RegistrationUser(RegistrationRequest registrationData)
        {
            var user = await _service.RegistrateUser(registrationData);

            var responce = user?.ToDto();

            return (user is null)
                ? BadRequest()
                : Created("", responce);
        }

        [Authorize]
        [HttpGet("CheckToken")]
        public ActionResult ValidateToken()
            => Ok(new
            {
                Message = "Токен действует",
                Date = DateTime.UtcNow
            });
    }
}
