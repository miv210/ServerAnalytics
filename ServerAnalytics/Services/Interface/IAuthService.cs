using Microsoft.AspNetCore.Mvc;
using ServerAnalytics.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ServerAnalytics.Services.Interface
{
    public interface IAuthService
    {
        JsonResult GenerationJWT(User person);
    }
}
