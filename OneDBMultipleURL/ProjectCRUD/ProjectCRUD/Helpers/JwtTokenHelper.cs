using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace ProjectCRUD.Helpers
{
    public class JwtTokenHelper
    {
        public static string GenerateEncryptedToken(string userId, string jwtSecretKey, string jwtIssuer, string jwtAudience, int expireMinutes = 15)
        {
            var key = Encoding.UTF8.GetBytes(jwtSecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string plainJwt = tokenHandler.WriteToken(token);

            string aesKey = "MyString12345678";
            
            return Encrypt(plainJwt, aesKey);
        }

        public static string Encrypt(string plainText, string keyHere)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(keyHere);
            aes.IV = new byte[16];

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (StreamWriter sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            string base64 = Convert.ToBase64String(ms.ToArray());

            string urlSafe = base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
            return urlSafe;
        }

        public static string DecryptUrlSafeToken(string urlSafeToken, string keyHere)
        {
            string base64 = urlSafeToken.Replace("-", "+").Replace("_", "/");
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(keyHere);
            aes.IV = new byte[16];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64));
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
