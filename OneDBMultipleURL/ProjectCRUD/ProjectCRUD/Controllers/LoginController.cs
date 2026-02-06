using Microsoft.AspNetCore.Mvc;
using ProjectCRUD.Data;
using ProjectCRUD.Helpers;
using ProjectCRUD.Models;

namespace ProjectCRUD.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public LoginController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User loginUser)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input";
                return View();
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            string token = JwtTokenHelper.GenerateEncryptedToken(
                user.Id.ToString(),
                _config["Jwt:Key"],
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"]
            );

            ViewBag.Token = token;

            return View("Success");
        }
    }
}
