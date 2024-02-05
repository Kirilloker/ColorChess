using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class JwtBearerOptionsConfig
{
    public static Action<JwtBearerOptions> OptionsConfiguration()
    {
        return options => {
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
        };
    }
}
    