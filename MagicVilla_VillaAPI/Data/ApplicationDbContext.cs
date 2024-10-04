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
                    Details = "Experience the epitome of coastal living in this stunning 5-bedroom villa, nestled on a private stretch of beachfront. The villa boasts floor-to-ceiling windows with panoramic ocean views, an infinity pool overlooking the waves, and a spacious outdoor terrace perfect for sunset dinners. ",
                    ImageUrl = "https://images.pexels.com/photos/1732414/pexels-photo-1732414.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "sea view, smart home features and a private parking",
                    CreatedDate = DateTime.Now
                },
              new Villa
              {
                  Id = 2,
                  Name = "Premium Pool Villa",
                  Details = "Located in a prestigious gated community, this 5-bedroom family villa combines luxury with practicality. Perfect for golf enthusiasts, the villa provides direct access to an 18-hole championship golf course.",
                  ImageUrl = "https://images.pexels.com/photos/24805051/pexels-photo-24805051/free-photo-of-view-of-the-terrace-with-a-swimming-pool-at-the-resort.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 5,
                  Rate = 300,
                  Sqft = 550,
                  Amenity = "Landscaped Yard, private pool, Fitness Area",
                  CreatedDate = DateTime.Now
              },
              new Villa
              {
                  Id = 3,
                  Name = "Luxury Pool Villa",
                  Details = "Set in a serene, sought-after neighborhood, this modern 5-bedroom villa offers a perfect blend of luxury and comfort, surrounded by scenic countryside views.",
                  ImageUrl = "https://images.pexels.com/photos/53610/large-home-residential-house-architecture-53610.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 5,
                  Rate = 400,
                  Sqft = 750,
                  Amenity = "private pool, country side, gym",
                  CreatedDate = DateTime.Now
              },
              new Villa
              {
                  Id = 4,
                  Name = "Diamond Villa",
                  Details = "This beautifully designed 4-bedroom villa, perched on a hillside, offers breathtaking views of the surrounding mountains and valleys. The Mediterranean-style architecture features terracotta roofs, arched doorways, and large balconies that invite the outdoors in.",
                  ImageUrl = "https://images.pexels.com/photos/816198/pexels-photo-816198.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 4,
                  Rate = 550,
                  Sqft = 900,
                  Amenity = "Outdoor BBQ area, Garden or Landscaped Yard, Fully-equipped kitchen",
                  CreatedDate = DateTime.Now
              },
              new Villa
              {
                  Id = 5,
                  Name = "Diamond Pool Villa",
                  Details = "This state-of-the-art 6-bedroom villa offers sleek, modern design paired with cutting-edge smart home technology. Set on a spacious plot with manicured gardens and an infinity pool, this home is ideal for both relaxation and entertaining.",
                  ImageUrl = "https://images.pexels.com/photos/5746250/pexels-photo-5746250.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                  Occupancy = 6,
                  Rate = 600,
                  Sqft = 1100,
                  Amenity = "Landscaped Yard, private pool, Home Cinema",
                  CreatedDate = DateTime.Now
              });
                
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
    }
}
