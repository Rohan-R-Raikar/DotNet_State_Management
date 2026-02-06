using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectReport.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace ProjectReport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportingDbContext _context;
        private readonly IConfiguration _config;

        public ReportsController(ReportingDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("orders")]
        public IActionResult GetOrdersReport([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token is missing");

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
                return Unauthorized("Invalid encrypted token");
            }

            int? userId = GetUserIdFromToken(decryptedToken);
            if (userId == null)
                return Unauthorized("Invalid token");

            var report = _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new { o.CustomerName, o.TotalAmount, o.OrderDate })
                .ToList();

            return Ok(report);
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
