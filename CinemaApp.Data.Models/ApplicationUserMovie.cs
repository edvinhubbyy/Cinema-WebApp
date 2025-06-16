using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data.Models
{

    [Comment("User Watchlist entry in the system")]
    public class ApplicationUserMovie
    {

        [Comment("Foreign key to tge references ApplicationUser. Part of the entity composite PK")]
        public string ApplicationUserId { get; set; } = null!;

        public virtual IdentityUser ApplicationUser { get; set; } = null!;

        [Comment("Foreign key to the references Movie.")]
        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
