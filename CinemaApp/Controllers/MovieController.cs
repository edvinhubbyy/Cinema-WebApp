using CinemaApp.Web.ViewModels;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using static CinemaApp.GCommon.Movie;

namespace CinemaApp.Web.Controllers;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<AllMoviesIndexViewModel> allMovies = await this._movieService.GetAllMoviesTaskAsync();

        return View(allMovies);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(MovieFormInputModel inputModel)
    {

        if (!ModelState.IsValid)
        {
            return View(inputModel);
        }

        try
        {
            await this._movieService.AddMovieAsync(inputModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            this.ModelState.AddModelError(string.Empty, ServiceCreatingError);
            return this.View(inputModel);
        }



    }

    [HttpGet]
    public async Task<IActionResult> Details(string? id)
    {

        try
        {
            MovieDetailsViewModel? movieDetails = await this._movieService.GetMovieDetailsByIdAsync(id);

            if (movieDetails == null)
            {
                // TODO: Custom 404 page
                return this.RedirectToAction(nameof(Index));
            }

            return View(movieDetails);
        }
        catch (Exception e)
        {
            // TODO: Implement a logger to log the error
            // TODO:  Add JS bars to indicate that the movie was not found
            Console.WriteLine(e.Message);
            return this.RedirectToAction(nameof(Index));
        }

    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {

        try
        {
            MovieFormInputModel? editableMovie = await this._movieService.GetMovieForEditAsync(id);

            if (editableMovie == null)
            {
                return this.RedirectToAction(nameof(Index));
            }
            return View(editableMovie);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return this.RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]

    public async Task<IActionResult> Edit(MovieFormInputModel inputModel)
    {
        if (!ModelState.IsValid)
        {
            return View(inputModel);
        }
        try
        {
            bool editSuccess = await this._movieService.EditMovieAsync(inputModel);

            if (!editSuccess)
            {
                // TODO: Custom 404 page
                return this.RedirectToAction(nameof(Index));
            }
            return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return this.RedirectToAction(nameof(Index));
        }
    }

}