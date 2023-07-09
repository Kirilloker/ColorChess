using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

Config.LoadConfig();

DB.ClearLobby();
DB.ClearRoom();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerOptionsConfig.OptionsConfiguration());

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<GameHub>("/Game");
    endpoints.MapPost("/login", LoginAndRegistry.Login);
    endpoints.MapPost("/registry", LoginAndRegistry.Registry);
});


app.Run("http://" + Config.IpServer + ":11000");

