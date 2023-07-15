using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class LoginAndRegistry
{
    public static async Task<IResult> Login(HttpContext context)
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
    }

    public static async Task<IResult> Registry(HttpContext context)
    {
        using StreamReader reader = new StreamReader(context.Request.Body);
        string text = await reader.ReadToEndAsync();
        string name = text.Split(" ")[0];
        string password = text.Split(" ")[1];

        User user = DB.GetUser(name);

        if (user == null)
        {
            User newUser = new User { Name = name, Password = password };
            DB.AddUser(newUser);

            newUser = DB.GetUser(newUser.Name);

            UserStatistic userStatistic = new UserStatistic { Win = 0, Lose = 0, Draw = 0, MaxScore = 0, Rate = 0, UserId = newUser.Id };
            DB.AddUserStatistic(userStatistic);

            return Results.Ok();
        }
        else
        {
            return Results.UnprocessableEntity();
        }

    }
}

//endpoints.MapPost("/login", async (HttpContext context) =>
//{

//    using StreamReader reader = new StreamReader(context.Request.Body);
//    string text = await reader.ReadToEndAsync();
//    string name = text.Split(" ")[0];
//    string password = text.Split(" ")[1];

//    User user = DB.GetUser(name);

//    if (user is null) return Results.Unauthorized();
//    if (user.Password != password) return Results.Unauthorized();

//    var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.UserData, name), new Claim(ClaimTypes.UserData, password) };


//    var jwt = new JwtSecurityToken(
//          issuer: AuthOptions.ISSUER,
//          audience: AuthOptions.AUDIENCE,
//          claims: claims,
//          expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
//          signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
//    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
//    var response = new
//    {
//        access_token = encodedJwt,
//    };
//    return Results.Json(response);
//});
//endpoints.MapPost("/registry", async (HttpContext context) =>
//{
//    using StreamReader reader = new StreamReader(context.Request.Body);
//    string text = await reader.ReadToEndAsync();
//    string name = text.Split(" ")[0];
//    string password = text.Split(" ")[1];

//    User user = DB.GetUser(name);

//    if (user == null)
//    {
//        User newUser = new User { Name = name, Password = password };
//        DB.AddUser(newUser);

//        newUser = DB.GetUser(newUser.Name);

//        UserStatistic userStatistic = new UserStatistic { Win = 0, Lose = 0, Draw = 0, MaxScore = 0, Rate = 0, UserId = newUser.Id };
//        DB.AddUserStatistic(userStatistic);

//        return Results.Ok();
//    }
//    else
//    {
//        return Results.UnprocessableEntity();
//    }
//});