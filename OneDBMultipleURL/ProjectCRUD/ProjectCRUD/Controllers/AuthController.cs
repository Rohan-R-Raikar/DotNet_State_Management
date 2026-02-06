using Microsoft.AspNetCore.Mvc;
using ProjectCRUD.Data;
using ProjectCRUD.Helpers;
using ProjectCRUD.Models;

namespace ProjectCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            string token = JwtTokenHelper.GenerateEncryptedToken(
                user.Id.ToString(),
                _config["Jwt:Key"],
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"]
            );

            return Ok(new { Token = token });
        }
    }
}
