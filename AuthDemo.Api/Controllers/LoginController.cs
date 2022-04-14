using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public async Task<string> NoLogin()
        {
            return "你还没登录";
        }
        [HttpGet]
        public async Task<string> LoginSuccess(string userName,string password)
        {
            if (userName=="Wzz"&&password=="666")
            {
                ClaimsIdentity claimsIdentity=new("Ctm");
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name,userName));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier,"1"));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
                return "登录成功";
            }
            return "登录失败";
        }
    }
}