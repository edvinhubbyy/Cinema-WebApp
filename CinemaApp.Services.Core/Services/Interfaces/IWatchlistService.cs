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

        Task<bool> RemoveMovieFromWatchlistAsync(string? movieId, string? userId);

        Task<bool> IsMovieAddedToWatchlist(string? movieId, string? userId);

    }
}
