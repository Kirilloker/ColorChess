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
            OnAuthenticationFailed = (context) =>
            {
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            }
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = (context) =>
            {
                var accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""); ;
                var path = context.HttpContext.Request.Path;
                if (path.StartsWithSegments("/Game"))
                {
                    context.Token = accessToken;
                    context.Options.Validate();
                }
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = AuthOptions.ISSUER,
            ValidateIssuer = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<GameHub>("/Game");
    endpoints.MapPost("/login", async (HttpContext context) =>
    {

        using StreamReader reader = new StreamReader(context.Request.Body);
        string text = await reader.ReadToEndAsync();
        string name = text.Split(" ")[0];
        string password = text.Split(" ")[1];

        User user = DB.GetUser(name);

        if (user is null) return Results.Unauthorized();
        if (user.Password != password) return Results.Unauthorized();

        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.UserData, name), new Claim(ClaimTypes.UserData, password) };


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
});


app.Run("http://192.168.1.116:11000");

