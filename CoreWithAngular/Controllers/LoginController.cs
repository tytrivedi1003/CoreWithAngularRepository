using System.Threading.Tasks;
using Data;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreWithAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        IConfiguration _config;
        IUserDetails _userDetailsRepo;
        ILoginService _loginService;
        
        public LoginController(IConfiguration config, IUserDetails userDetailsRepo, ILoginService loginService)
        {
            _config = config;
            _userDetailsRepo = userDetailsRepo;
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDetails userModel,string pass)
        {
            var objUserModel = _userDetailsRepo.Login(userModel.EmailId,pass);
            string generatedToken = await _loginService.CreateToken(userModel);
            HttpContext.Session.SetString("JWToken", generatedToken);
            return Ok(new { token = generatedToken, email = userModel.EmailId, fullname = userModel.FirstName });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDetails userDetails, string pass)
        {
            userDetails.EmailId = userDetails.EmailId.ToLower();
            if (await _userDetailsRepo.UserExists(userDetails.EmailId))
                return BadRequest("Email already exists");

            var createdUser = await _userDetailsRepo.Register(userDetails, pass);
            return StatusCode(201, new { email = createdUser.EmailId, fullname = createdUser.FirstName });
        }
    }
}