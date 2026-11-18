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
    public class LoginController(LoginService service) : ControllerBase
    {
        private readonly LoginService _service = service;

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
        public async Task<ActionResult<bool>> ValidateToken()
            => NoContent();
    }
}
