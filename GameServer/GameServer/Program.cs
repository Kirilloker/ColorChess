using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

const string baseIP = "192.168.1.116";
//const string baseIP = "172.20.10.10";

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

app.Run("http://" + baseIP + ":11000");