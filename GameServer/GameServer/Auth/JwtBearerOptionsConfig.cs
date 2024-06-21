using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class JwtBearerOptionsConfig
{
    public static Action<JwtBearerOptions> OptionsConfiguration()
    {
        return options => 
        {
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = OnAuthenticationFailedAsync,
                OnMessageReceived = OnMessageReceivedAsync,
            };

            options.TokenValidationParameters = GetTokenValidationParameters();
        };
    }


    private static async Task OnMessageReceivedAsync(MessageReceivedContext context)
    {
        var authorizationHeader = context.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var accessToken = authorizationHeader.Replace("Bearer ", "");
            var path = context.HttpContext.Request.Path;

            if (path.StartsWithSegments("/Game"))
            {
                context.Token = accessToken;
                context.Options.Validate();
            }
        }
        else
        {
            await Console.Out.WriteLineAsync("Authorization header is empty");
        }

        await Task.CompletedTask;
    }

    private static async Task OnAuthenticationFailedAsync(AuthenticationFailedContext context)
    {
        await Console.Out.WriteLineAsync("Authentication was failed: " + context.Exception.Message);
        await Task.CompletedTask;
    }

    private static TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidIssuer = AuthOptions.ISSUER,
            ValidateIssuer = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    }
}
