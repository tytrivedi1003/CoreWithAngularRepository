using System;
using System.Threading.Tasks;
using Data;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace CoreWithAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserDetails _userDetailsRepo;
        private readonly ILoginService _loginService;
        private readonly IDistributedCache _distributedCache;

        public LoginController(IConfiguration config, IUserDetails userDetailsRepo, ILoginService loginService, IDistributedCache distributedCache)
        {
            _config = config;
            _userDetailsRepo = userDetailsRepo;
            _loginService = loginService;
            _distributedCache = distributedCache;
        }
        [EnableCors("CORS")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserDetails userModel)
        {
            string pass=  string.Empty;
            var objUserModel = _userDetailsRepo.Login(userModel.EmailId,pass);
            string generatedToken = await _loginService.CreateToken(userModel);
            HttpContext.Session.SetString("JWToken", generatedToken);
            return Ok(new { token = generatedToken, email = userModel.EmailId, fullname = userModel.FirstName });
        }

        [EnableCors("CORS")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]Users userData)
        {
            string pass = string.Empty;
            userData.EmailId = userData.EmailId.ToLower();
            if (await _userDetailsRepo.UserExists(userData.EmailId))
                return BadRequest("Email already exists");
            if (userData != null)
            {
                UserDetails userDetails = new UserDetails();
                userDetails.FirstName = userData.FirstName;
                userDetails.LastName = userData.LastName;
                userDetails.EmailId = userData.EmailId;
                userDetails.Phone = userData.Phone;
                await _userDetailsRepo.Register(userDetails, userData.StrPass);
            }
            
            return StatusCode(201, new { email = userData.EmailId, fullname = userData.FirstName });
        }

        [HttpGet("GetCache")]
        public async Task<string> GetCache()
        {
            string existingTime = string.Empty;
            try
            {
                var cacheKey = "TheTime";
                existingTime = _distributedCache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(existingTime))
                {
                    return "Fetched from cache : " + existingTime;
                }
                else
                {
                    existingTime = DateTime.UtcNow.ToString();
                    _distributedCache.SetString(cacheKey, existingTime);
                    return "Added to cache : " + existingTime;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}