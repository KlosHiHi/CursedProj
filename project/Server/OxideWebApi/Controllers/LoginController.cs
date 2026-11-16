using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OxideClassLibrary.DTOs;
using OxideWebApi.Services;

namespace OxideWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(LoginService service) : ControllerBase
    {
        private readonly LoginService _service = service;

        [AllowAnonymous]
        [HttpPost("authorize")]
        public async Task<ActionResult<string>> AuthEmployer(LoginRequest loginData)
        {
            var login = loginData.Login;
            var password = loginData.Password;

            var result = await _service.AuthEmployerAsync(login, password);

            return result is null
                ? Unauthorized()
                : result;
        }
    }
}
