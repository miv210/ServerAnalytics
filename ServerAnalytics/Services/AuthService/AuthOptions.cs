using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ServerAnalytics.Services.AuthService
{
    public class AuthOptions
    {
        public const string ISSUER = "RxGroup"; // издатель токена
        public const string AUDIENCE = "ServerAnalytics"; // потребитель токена
        const string KEY = "#23SecretKey45!";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
