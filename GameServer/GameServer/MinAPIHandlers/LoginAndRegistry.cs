using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class LoginAndRegistry
{
    public static async Task<IResult> Login(HttpContext context)
    {
        string text = await ReadRequestBodyAsync(context);
        var (name, password) = ExtractCredentials(text);

        if (!ValidateUser(name, password, out User? user) || user == null)
            return Results.Unauthorized();

        string token = GenerateJwt(user);

        DB.IDK_how_fix_this_bug(user.Id);
        LogToDB(user, LogEventType.Authorization);

        var response = new
        {
            access_token = token,
        };

        return Results.Json(response);
    }

    public static async Task<IResult> Registry(HttpContext context)
    {
        string text = await ReadRequestBodyAsync(context);
        var (name, password) = ExtractCredentials(text);

        if (DB.GetUser(name) != null)
            return Results.UnprocessableEntity();

        User newUser = new User { Name = name, Password = password };

        int userId = DB.AddEntity(newUser) ?? throw new InvalidOperationException("Failed to add user");

        UserStatistic userStatistic = new UserStatistic { Win = 0, Lose = 0, Draw = 0, MaxScore = 0, Rate = 0, UserId = userId };

        int userStatisticId = DB.AddEntity(userStatistic) ?? throw new InvalidOperationException("Failed to add user statistic");

        LogToDB(newUser, LogEventType.Registration);

        return Results.Ok();
    }



    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        using StreamReader reader = new StreamReader(context.Request.Body);
        return await reader.ReadToEndAsync();
    }

    private static (string name, string password) ExtractCredentials(string text)
    {
        var parts = text.Split(" ");
        return (parts[0], parts[1]);
    }

    private static bool ValidateUser(string name, string password, out User? user)
    {
        user = DB.GetUser(name); 

        return user != null && user.Password == password;
    }

    private static void LogToDB(User user, LogEventType logEvent)
    {
        string message = logEvent == LogEventType.Authorization ? "Authorization user" : "Registration new user";

        DB.AddLogEvent(logEvent, user.Id, message);
    }

    private static string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.UserData, user.Name),
            new Claim(ClaimTypes.UserData, user.Password)
        };

        var jwt = new JwtSecurityToken(
              issuer: AuthOptions.ISSUER,
              audience: AuthOptions.AUDIENCE,
              claims: claims,
              expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
              signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}