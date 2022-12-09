using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/Game")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });



var app = builder.Build();


app.MapPost("/login", () =>
{
    var user = "Data from db";

    if (user is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "name from db") };

    var jwt = new JwtSecurityToken(
          issuer: AuthOptions.ISSUER,
          audience: AuthOptions.AUDIENCE,
          claims: claims,
          expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
          signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
    var response = new
    {
        access_token = encodedJwt,
    };

    return Results.Json(response);
});


app.MapHub<GameHub>("/Game");


app.Run("http://localhost:11000");