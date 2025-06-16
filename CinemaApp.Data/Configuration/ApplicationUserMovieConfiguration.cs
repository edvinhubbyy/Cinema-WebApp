using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserMovie> entity)
        {
            entity
                .HasKey(um => new { um.ApplicationUserId, um.MovieId });

            // Configuring the properties of ApplicationUserMovie
            entity
                .Property(um => um.ApplicationUserId)
                .IsRequired();


            // Define default value for IsDeleted property
            entity
                .Property(um => um.IsDeleted)
                .HasDefaultValue(false);

            // Configuring the relationship between ApplicationUserMovie and Movie
            // The IdentityUser does not contain a navigation property for ApplicationUserMovie as it is built-in type of ASP.NET Core Identity
            entity
                .HasOne(aum => aum.ApplicationUser)
                .WithMany()
                .HasForeignKey(aum => aum.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between ApplicationUserMovie and Movie
            entity
                .HasOne(aum => aum.Movie)
                .WithMany(m => m.UserWatchLists)
                .HasForeignKey(aum => aum.MovieId);

            // Defining a global query filter to exclude soft-deleted movies
            entity
                .HasQueryFilter(aum => aum.Movie.IsDeleted == false);


            // Defining a global query filter to exclude soft-deleted entries
            entity
                .HasQueryFilter(aum => aum.IsDeleted == false);
        }
    }
}
