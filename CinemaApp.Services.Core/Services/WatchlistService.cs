using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Services.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.GCommon.EntityConstants;

namespace CinemaApp.Services.Core.Services
{
    public class WatchlistService : IWatchlistService
    {

        private readonly CinemaAppDbContext dbContext;

        public WatchlistService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId)
        {
            IEnumerable<WatchlistViewModel> userWatchlist = await this.dbContext
                .ApplicationUserMovies
                .Include(aum => aum.Movie)
                .AsNoTracking()
                .Where(aum => aum.ApplicationUserId.ToLower() == userId.ToLower())
                .Select(aum => new WatchlistViewModel()
                {
                    MovieId = aum.Movie.Id.ToString(),
                    Title = aum.Movie.Title,
                    Genre = aum.Movie.Genre,
                    ReleaseDate = aum.Movie.ReleaseDate.ToString(AppDateFormat),
                    ImageUrl = aum.Movie.ImageUrl ?? $"/images/{NoImageUrl}"
                })
                .ToArrayAsync();

            return userWatchlist;
        }

        public async Task<bool> AddMovieToUserWatchlistAsync(string? movieId, string? userId)
        {

            bool result = false;
            if (movieId != null && userId != null)
            {
                bool isMovieIdValidGuid = Guid.TryParse(movieId, out Guid movieGuid);

                if (isMovieIdValidGuid)
                {
                    ApplicationUserMovie? userMovieEntry = await this.dbContext
                        .ApplicationUserMovies
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(aum => 
                            aum.ApplicationUserId.ToLower() == userId 
                                                     && aum.MovieId.ToString() == movieGuid.ToString());
                    if (userMovieEntry != null)
                    {
                        userMovieEntry.IsDeleted = false;
                    }
                    else
                    {
                        userMovieEntry = new ApplicationUserMovie()
                        {
                            ApplicationUserId = userId,
                            MovieId = movieGuid,
                        };
                        await this.dbContext.ApplicationUserMovies.AddAsync(userMovieEntry);
                    }
                    await this.dbContext.SaveChangesAsync();
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> RemoveMovieFromUserWatchlistAsync(string? movieId, string? userId)
        {
            bool result = false;
            if (movieId != null && userId != null)
            {
                bool isMovieIdValidGuid = Guid.TryParse(movieId, out Guid movieGuid);

                if (isMovieIdValidGuid)
                {
                    ApplicationUserMovie? userMovieEntry = await this.dbContext
                        .ApplicationUserMovies
                        .SingleOrDefaultAsync(aum =>
                            aum.ApplicationUserId.ToLower() == userId
                            && aum.MovieId.ToString() == movieGuid.ToString());
                    if (userMovieEntry != null)
                    {
                        userMovieEntry.IsDeleted = true;

                        await this.dbContext.SaveChangesAsync();

                        result = true;
                    }

                }
            }

            return result;
        }

        public async Task<bool> IsMovieAddedToWatchlistAsync(string? movieId, string? userId)
        {
            bool result = false;
            if (movieId != null && userId != null)
            {
                bool isMovieIdValidGuid = Guid.TryParse(movieId, out Guid movieGuid);

                if (isMovieIdValidGuid)
                {
                    ApplicationUserMovie? userMovieEntry = await this.dbContext
                        .ApplicationUserMovies
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(aum =>
                            aum.ApplicationUserId.ToLower() == userId
                            && aum.MovieId.ToString() == movieGuid.ToString());

                    if (userMovieEntry != null)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
