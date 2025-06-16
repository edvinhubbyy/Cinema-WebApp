using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Web.ViewModels.Watchlist;

namespace CinemaApp.Services.Core.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId);

        Task<bool> AddMovieToUserWatchlistAsync(string? movieId, string? userId);

        Task<bool> RemoveMovieFromUserWatchlistAsync(string? movieId, string? userId);

        Task<bool> IsMovieAddedToWatchlistAsync(string? movieId, string? userId);

    }
}
