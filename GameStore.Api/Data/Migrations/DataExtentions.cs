using System;
using System.Runtime.CompilerServices;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data.Migrations;

public static class DataExtentions
{
    public static void MigrateDb(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }


    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString
, optionsAction: options => options.UseSeeding((context, _) =>
{
    if(!context.Set<Genre>().Any())
    {
        context.Set<Genre>().AddRange(
            new Genre{Id=1, Title="Fighting"},
            new Genre{Id=2, Title="RPG"},
            new Genre{Id=3, Title="Platformer"},
            new Genre{Id=4, Title="Racing"}
        );
        context.SaveChanges();
    }
}));
    }

}
