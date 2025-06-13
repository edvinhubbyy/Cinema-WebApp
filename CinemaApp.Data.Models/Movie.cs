namespace CinemaApp.Data.Models;
using static CinemaApp.GCommon.EntityConstants;

public class Movie
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public string Director { get; set; } = null!;

    public int Duration { get; set; }

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; }
        = $"~/images/{NoImageUrl}";

    public bool IsDeleted { get; set; }
}