using FirstEF6App;
using GameServer;
using GameServer.GameServer.Hubs;
using GameServer.GameServer.MinAPIHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

Config.LoadConfig();
    
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerOptionsConfig.OptionsConfiguration());


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Color Chess API", Version = "v1.2" });
    c.EnableAnnotations();
});

builder.Services.AddDbContext<ColorChessContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color Chess API");
    });
}
app.MapControllers();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<GameServerHub>("/Game");
    endpoints.MapPost("/login", (HttpContext context) => {  LoginAndRegistry.Login(context).Result.ExecuteAsync(context);});
    endpoints.MapPost("/registry", (HttpContext context) => {  LoginAndRegistry.Registry(context).Result.ExecuteAsync(context);});
});


string URL_server = $"http://{Config.IpServer}:{Config.PortServer}";

Console.WriteLine($"Start Server {URL_server}");
app.Run(URL_server);


// http://192.168.0.35:11000/swagger/index.html