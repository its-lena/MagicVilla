using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://images.pexels.com/photos/1732414/pexels-photo-1732414.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                    Occupancy = 4,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
              new Villa
              {
                  Id = 2,
                  Name = "Premium Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://images.pexels.com/photos/24805051/pexels-photo-24805051/free-photo-of-view-of-the-terrace-with-a-swimming-pool-at-the-resort.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 4,
                  Rate = 300,
                  Sqft = 550,
                  Amenity = "",
                  CreatedDate = DateTime.Now
              },
              new Villa
              {
                  Id = 3,
                  Name = "Luxury Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://images.pexels.com/photos/53610/large-home-residential-house-architecture-53610.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 4,
                  Rate = 400,
                  Sqft = 750,
                  Amenity = "",
                  CreatedDate = DateTime.Now
              },
              new Villa
              {
                  Id = 4,
                  Name = "Diamond Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://images.pexels.com/photos/816198/pexels-photo-816198.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 4,
                  Rate = 550,
                  Sqft = 900,
                  Amenity = "",
                  CreatedDate = DateTime.Now
              },
              new Villa
              {
                  Id = 5,
                  Name = "Diamond Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://images.pexels.com/photos/5746250/pexels-photo-5746250.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 4,
                  Rate = 600,
                  Sqft = 1100,
                  Amenity = "",
                  CreatedDate = DateTime.Now
              });
                
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
    }
}
