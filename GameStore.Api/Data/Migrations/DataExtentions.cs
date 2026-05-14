using System;
using System.Runtime.CompilerServices;
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

}
