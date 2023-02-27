using System.IdentityModel.Tokens.Jwt;

namespace ServerAnalytics.Services.AuthService
{
    public struct JwtResponse
    {
        public JwtSecurityToken Token {get; set;}
        public string Login { get; set;}

    }
}
