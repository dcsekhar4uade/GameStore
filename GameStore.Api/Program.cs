using GameStore.Api.Data;
using GameStore.Api.Data.Migrations;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();
app.MigrateDb();

app.Run();
