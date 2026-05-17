using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateUpdateGameDto(
    [Required][MaxLength(100)]
    string Name,

    [Range(1,50)]
    int GenreId,

    [Required][Range(1,100)]
    decimal Price,

    DateOnly ReleaseDate
);
