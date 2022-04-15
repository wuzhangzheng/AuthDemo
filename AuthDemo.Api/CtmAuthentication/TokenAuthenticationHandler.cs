using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace AuthDemo.Api.CtmAuthentication
{
    public class TokenAuthenticationHandler : IAuthenticationHandler
    {
        private AuthenticationScheme _scheme;
        private HttpContext _context;
        /// <summary>
        /// 鉴权时的初始化
        /// </summary>
        /// <param name="scheme">鉴权架构名称</param>
        /// <param name="context">HttpContext</param>
        public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this._context = context;
            this._scheme = scheme;
        }
        /// <summary>
        /// 鉴权
        /// </summary>
        public Task<AuthenticateResult> AuthenticateAsync()
        { 
            string token=_context.Request.Headers["Authorization"];
            if (token=="Wzz")
            {
                ClaimsIdentity claimsIdentity=new("Ctm");
                claimsIdentity.AddClaims(new List<Claim>{
                    new Claim(ClaimTypes.Name,"Wzz"),
                    new Claim(ClaimTypes.NameIdentifier,"6")
                });
                var claimsPrincipal =new ClaimsPrincipal(claimsIdentity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal,null,_scheme.Name)));
            }
            return Task.FromResult(AuthenticateResult.Fail($"token错误，请重新登录"));
        }
        /// <summary>
        /// 未登录
        /// </summary>
        /// <param name="properties"></param>
        public async Task ChallengeAsync(AuthenticationProperties? properties)
        {
            _context.Response.Redirect("/login/NoLogin");
        }
        /// <summary>
        /// 没权限访问
        /// </summary>
        /// <param name="properties"></param>
        public async Task ForbidAsync(AuthenticationProperties? properties)
        {
            _context.Response.StatusCode=403;
        }
    }
}