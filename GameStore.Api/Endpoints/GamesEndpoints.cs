using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    static readonly List<GameDto> games = [
        new (1, "Street Fighter II", "Action",  19.99m, new DateOnly(1992, 7, 15)),
        new (2, "Street Fighter",    "Action",  10.99m, new DateOnly(1989, 7, 15)),
        new (3, "Titanic",           "Romantic",11.29m, new DateOnly(2000, 7, 15)),
        new (4, "MBI",               "Comedy",  12.49m, new DateOnly(2005, 7, 15)),
    ];

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");


        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        group.MapPost("", (CreateUpdateGameDto newGame) =>
        {
            GameDto game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (int id, CreateUpdateGameDto updatedGame) =>
        {
            int index = games.FindIndex(g => g.Id == id);
            if (index == -1) return Results.NotFound();

            games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
            return Results.Ok(games[index]);
        });

        group.MapDelete("/{id}", (int id) =>
        {
            int index = games.FindIndex(g => g.Id == id);
            if (index != -1) games.RemoveAt(index);
            return Results.Ok(games);
        });

        return app;
    }
}
