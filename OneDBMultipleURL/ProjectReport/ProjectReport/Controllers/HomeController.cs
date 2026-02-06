using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectReport.Models;
using ProjectReport.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace ProjectReport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReportingDbContext _context;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, ReportingDbContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        public IActionResult Index(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Token is missing. Please login first.";
                return View(new List<Order>());
            }

            string decryptedToken;
            try
            {
                string decodedToken = WebUtility.UrlDecode(token);
                decodedToken = decodedToken.Replace("-", "+").Replace("_", "/");
                decodedToken = FixBase64Padding(decodedToken);

                decryptedToken = Decrypt(decodedToken);
            }
            catch
            {
                ViewBag.Error = "Invalid encrypted token.";
                return View(new List<Order>());
            }

            int? userId = GetUserIdFromToken(decryptedToken);
            if (userId == null)
            {
                ViewBag.Error = "Invalid token. Could not extract user.";
                return View(new List<Order>());
            }

            var report = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            return View(report);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int? GetUserIdFromToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                    return null;

                return int.Parse(userIdClaim.Value);
            }
            catch
            {
                return null;
            }
        }

        private string Decrypt(string cipherText)
        {
            string aesKey = "MyString12345678";

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(aesKey);
            aes.IV = new byte[16];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        private string FixBase64Padding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: return base64 + "==";
                case 3: return base64 + "=";
                default: return base64;
            }
        }
    }
}
