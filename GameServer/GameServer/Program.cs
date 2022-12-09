<<<<<<< Updated upstream
Console.WriteLine(DB.GetUser("kolinka"));
=======
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
>>>>>>> Stashed changes

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });


var app = builder.Build();


app.MapPost("/login", () =>
{
    var user = "что то что нашли в бд";

    //Если не нашли пользователя
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
