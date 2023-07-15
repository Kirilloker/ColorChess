using GameServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

Config.LoadConfig();

DB.AddUser("user1", "password1");
DB.AddUserStatistic(1, 1, 1, 1, 1, 3);

DB.AddUser("user2", "password2");
DB.AddUserStatistic(1, 1, 1, 1, 1, 4);


DB.AddUser("user3", "password3");
DB.AddUserStatistic(1, 1, 1, 1, 1, 5);

DB.AddUser("user4", "password4");
DB.AddUserStatistic(1, 1, 1, 1, 1, 6);



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
    endpoints.MapHub<GameServerHub>("/Game");
    endpoints.MapPost("/login", (HttpContext context) => {  LoginAndRegistry.Login(context).Result.ExecuteAsync(context);});
    endpoints.MapPost("/registry", (HttpContext context) => {  LoginAndRegistry.Registry(context).Result.ExecuteAsync(context);});
    endpoints.MapGet("/top", (HttpContext context) => { Test.GetTop(context).Result.ExecuteAsync(context); });
    endpoints.MapGet("/placeInTop", (HttpContext context) => { Test.GetNumberPlaceTop(context).Result.ExecuteAsync(context); });
});


app.Run("http://" + Config.IpServer + ":11000");

