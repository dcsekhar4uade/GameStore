using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");


        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                .Select(g => new GameDetailsDto(g.Id, g.Title, g.GenreId, g.Price, g.ReleaseDate))
                .ToListAsync()
        );

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is null
                ? Results.NotFound()
                : Results.Ok(new GameDetailsDto(game.Id, game.Title, game.GenreId, game.Price, game.ReleaseDate));
        }).WithName(GetGameEndpointName);

        group.MapPost("", async (CreateUpdateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Title = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Title,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        group.MapPut("/{id}", async (int id, CreateUpdateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            if (game is null) return Results.NotFound();

            game.Title = newGame.Name;
            game.GenreId = newGame.GenreId;
            game.Price = newGame.Price;
            game.ReleaseDate = newGame.ReleaseDate;

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return app;
    }
}
