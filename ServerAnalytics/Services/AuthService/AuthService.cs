using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ServerAnalytics.Services;
using ServerAnalytics.Services.Interface;
using ServerAnalytics.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ServerAnalytics.Services.AuthService
{
    public class AuthService : IAuthService
    {
        JwtResponse jwtResponse = new JwtResponse();
        User user1= new User();
        
        public JsonResult GenerationJWT(User person)
        {
            user1.Login = "admin";
            user1.Password = "admin";
            user1.Role = "admin";

            User user;
            using(ServerAnalyticsContext db = new ServerAnalyticsContext())
            {
                 user = db.Users.FirstOrDefault(p => p.Login == person.Login && p.Password == person.Password);
            }

            if(user is null)
            {
                return (JsonResult)Results.Unauthorized();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            jwtResponse.Token = jwt;
            jwtResponse.Login = person.Login;

            return new JsonResult(jwtResponse) ;
        }
    }
}
