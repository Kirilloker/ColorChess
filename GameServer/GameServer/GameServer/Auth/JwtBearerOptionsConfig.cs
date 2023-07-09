using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
    

//.AddJwtBearer(options =>
// {
//     options.Events = new JwtBearerEvents
//     {
//         OnAuthenticationFailed = (context) =>
//         {
//             Console.WriteLine(context.Exception.Message);
//             return Task.CompletedTask;
//         }
//     };

//     options.Events = new JwtBearerEvents
//     {
//         OnMessageReceived = (context) =>
//         {
//             var accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""); ;
//             var path = context.HttpContext.Request.Path;
//             if (path.StartsWithSegments("/Game"))
//             {
//                 context.Token = accessToken;
//                 context.Options.Validate();
//             }
//             return Task.CompletedTask;
//         }
//     };

//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidIssuer = AuthOptions.ISSUER,
//         ValidateIssuer = true,
//         ValidAudience = AuthOptions.AUDIENCE,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//         ValidateIssuerSigningKey = true,
//     };
// });


