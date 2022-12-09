Console.WriteLine(DB.GetUser("kolinka"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<ChatHub>("/Chat");


app.Run("http://localhost:11000");
