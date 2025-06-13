using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels;
using CinemaApp.Web.ViewModels.Movie;

public interface IMovieService
{
    Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesTaskAsync();

    Task AddMovieAsync(MovieFormInputModel inputModel);

    Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id);

    Task<MovieFormInputModel?> GetMovieForEditAsync(string id);

    Task<bool> EditMovieAsync(MovieFormInputModel inputModel);

    Task<DeleteMovieViewModel?> GetMovieDeleteDetailsByIdAsync(string? id);

    Task<bool> SoftDeleteMovieAsync(string? id);

    Task<bool> DeleteMovieAsync(string? id);




}