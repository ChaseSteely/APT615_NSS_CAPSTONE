using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APT615.Models;

namespace APT615.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Aminity> Aminities { get; set; }
        public DbSet<ApartmentAmenity> ApartmentAmenities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Apartment>().ToTable("Apartment");
            builder.Entity<Aminity>().ToTable("Amenity");
            builder.Entity<ApartmentAmenity>().ToTable("ApartmentAmenity");
            builder.Entity<Apartment>()
                            .Property(b => b.DateAdded)
                            .HasDefaultValueSql("GETDATE()");
        }
    }
}
