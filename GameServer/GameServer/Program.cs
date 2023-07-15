using GameServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

Config.LoadConfig();


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
    //endpoints.MapHub<GameHub>("/Game");
    endpoints.MapPost("/login", (HttpContext context) => {  LoginAndRegistry.Login(context).Result.ExecuteAsync(context);});
    endpoints.MapPost("/registry", (HttpContext context) => {  LoginAndRegistry.Registry(context).Result.ExecuteAsync(context);});
    endpoints.MapGet("/top", (HttpContext context) => { Test.GetNumberPlaceTop(context).Result.ExecuteAsync(context); });
    endpoints.MapGet("/placeInTop", (HttpContext context) => { Test.GetNumberPlaceTop(context).Result.ExecuteAsync(context); });
});


app.Run("http://" + Config.IpServer + ":11000");

