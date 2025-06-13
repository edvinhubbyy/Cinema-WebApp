using System.Globalization;
using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.GCommon.EntityConstants;

public class MovieService : IMovieService
{
    private readonly CinemaAppDbContext _dbContext;

    public MovieService(CinemaAppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesTaskAsync()
    {
        IEnumerable<AllMoviesIndexViewModel> allMovies = await this._dbContext
            .Movies
            .AsNoTracking()
            .Select(m => new AllMoviesIndexViewModel
            {
                Id = m.Id.ToString(),
                Title = m.Title,
                Genre = m.Genre,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}"
            })
            .ToListAsync();

        return allMovies;
    }

    public async Task AddMovieAsync(MovieFormInputModel inputModel)
    {
        Movie newMovie = new Movie
        {
            Title = inputModel.Title,
            Genre = inputModel.Genre,
            Director = inputModel.Director,
            Description = inputModel.Description,
            Duration = inputModel.Duration,
            ImageUrl = inputModel.ImageUrl ?? $"~/images/{NoImageUrl}",
            ReleaseDate = DateOnly.ParseExact(inputModel.ReleaseDate, AppDateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None)
        };

        await _dbContext.Movies.AddAsync(newMovie);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id)
    {
        MovieDetailsViewModel? movieDetails = null;

        bool isIdValidGuid = Guid.TryParse(id, out Guid movieId);

        if (isIdValidGuid)
        {
            movieDetails = await this._dbContext
                .Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new MovieDetailsViewModel
                {
                    Id = m.Id.ToString(),
                    Description = m.Description,
                    Director = m.Director,
                    Duration = m.Duration,
                    Genre = m.Genre,
                    ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}",
                    ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                    Title = m.Title
                })
                .SingleOrDefaultAsync();
        }

        return movieDetails;

    }

    public async Task<MovieFormInputModel?> GetMovieForEditAsync(string id)
    {
        MovieFormInputModel? editableMovie = null;

        bool isIdValidGuid = Guid.TryParse(id, out Guid movieId);

        if (isIdValidGuid)
        {
            editableMovie = await this._dbContext
                .Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new MovieFormInputModel
                {
                    Description = m.Description,
                    Director = m.Director,
                    Duration = m.Duration,
                    Genre = m.Genre,
                    ImageUrl = m.ImageUrl ?? $"~/images/{NoImageUrl}",
                    ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                    Title = m.Title
                })
                .SingleOrDefaultAsync();
        }

        return editableMovie;
    }

    public async Task<bool> EditMovieAsync(MovieFormInputModel inputModel)
    {
        Movie? editableMovie = await this.FindMovieByStringId(inputModel.Id);

        if (editableMovie == null)
        {
            return false;
        }

        DateOnly movieReleaseDate = DateOnly.ParseExact(inputModel.ReleaseDate, AppDateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);

        editableMovie.Title = inputModel.Title;
        editableMovie.Description = inputModel.Description;
        editableMovie.Director = inputModel.Director;
        editableMovie.Duration = inputModel.Duration;
        editableMovie.Genre = inputModel.Genre;
        editableMovie.ImageUrl = inputModel.ImageUrl ?? $"~/images/{NoImageUrl}";
        editableMovie.ReleaseDate = movieReleaseDate;

        await this._dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<DeleteMovieViewModel?> GetMovieDeleteDetailsByIdAsync(string? id)
    {
        
        DeleteMovieViewModel? deleteMovieViewModel = null;

        Movie? movieToBeDeleted = await this.FindMovieByStringId(id);

        if (movieToBeDeleted != null)
        {
            deleteMovieViewModel = new DeleteMovieViewModel
            {
                Id = movieToBeDeleted.Id.ToString(),
                Title = movieToBeDeleted.Title.ToString(),
                ImageUrl = movieToBeDeleted.ImageUrl ?? $"~/images/{NoImageUrl}"
            };
        }

        return deleteMovieViewModel;
    }

    public async Task<bool> SoftDeleteMovieAsync(string? id)
    {

        Movie? movieToDelete = await this.FindMovieByStringId(id);

        if (movieToDelete == null)
        {
            return false;
        }

        // Soft Delete <=> Edit of IsDeleted property
        movieToDelete.IsDeleted = true;
        
        await this._dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteMovieAsync(string? id)
    {

        Movie? movieToDelete = await this.FindMovieByStringId(id);
        if (movieToDelete == null)
        {
            return false;
        }

        // TODO: To be investigated when deleting entities with relations
        this._dbContext.Movies.Remove(movieToDelete);
        await this._dbContext.SaveChangesAsync();

        return true;
    }

    // TODO: Implement as generic async method for finding entities by string Ids

    private async Task<Movie?> FindMovieByStringId(string? id)
    {

        Movie? movie = null;

        if (!string.IsNullOrWhiteSpace(id))
        {

            bool isIdValidGuid = Guid.TryParse(id, out Guid movieGuid);
            if (isIdValidGuid)
            {
                movie = await this._dbContext
                    .Movies
                    .FindAsync(movieGuid);
            }

        }
        return movie;


    }

}