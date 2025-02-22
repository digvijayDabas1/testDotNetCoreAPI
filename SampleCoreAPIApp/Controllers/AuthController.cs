using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SampleCoreAPIApp.IServices;
using SampleCoreAPIApp.Models.RequestModels;

namespace SampleCoreAPIApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userService;

        public AuthController(IUserServices userServices)
        {
            _userService = userServices;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            var response = _userService.Register(model);
            return Ok(response);
        }

        [HttpGet("get-profile")]
        [Authorize]
        public IActionResult GetProfile(int id)
        {
            var response = _userService.GetProfileDetails(id);
            return Ok(response);
        }
    }
}
