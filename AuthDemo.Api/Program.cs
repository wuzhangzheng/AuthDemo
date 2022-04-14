using System.ComponentModel;
using System.Security.Claims;
using AuthDemo.Api.Conts;
using AuthDemo.Api.CtmAuthentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//注册鉴权架构
#region cookie
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
// .AddCookie(o=>o.LoginPath="/Login/NoLogin");
#endregion
#region 自定义token验证
builder.Services.AddAuthentication(op =>
{
    //把自定义的鉴权方案添加到鉴权架构中
    op.AddScheme<TokenAuthenticationHandler>("token", "CtmToken");
    op.DefaultAuthenticateScheme = "token";
    op.DefaultChallengeScheme="token";
    op.DefaultForbidScheme="token";
});
#endregion
#region 授权
    builder.Services.AddAuthorization(op=>{
        // op.AddPolicy(AuthorizationConts.MYPOLICY,p=>p.RequireClaim(ClaimTypes.NameIdentifier,"6"));
        op.AddPolicy(AuthorizationConts.MYPOLICY, p =>p.RequireAssertion(ass=>ass.User.HasClaim(c=>c.Type==ClaimTypes.NameIdentifier) && ass.User.Claims.First(c=>c.Type.Equals(ClaimTypes.NameIdentifier)).Value == "6"));
    });
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
