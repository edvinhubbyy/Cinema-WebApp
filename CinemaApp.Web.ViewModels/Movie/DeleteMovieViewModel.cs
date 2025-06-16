using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Web.ViewModels.Movie
{
    public class DeleteMovieViewModel
    {

        [Required] 
        public string Id { get; set; } = null;

        public string? Title { get; set; }

        public string? ImageUrl { get; set; }
    }
}
