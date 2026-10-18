using Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCatWeightAgent();

var app = builder.Build();

app.MapCatWeightAgent();

app.Run();