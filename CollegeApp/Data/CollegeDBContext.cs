using CollegeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDBContext(DbContextOptions options): DbContext(options)
    {

       DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new
                {
                    Id = 1,
                    Name = "Venkat",
                    Email = "Venkat@gmail.com",
                    Address = "India",
                    DOB = new DateTime(2000, 1, 15)
                },
                new
                {
                    Id = 2,
                    Name = "Muath",
                    Email = "muath@gmail.com",
                    Address = "Jordan",
                    DOB = new DateTime(2003, 12, 16)
                }
                );
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(n => n.Name).IsRequired()
                .HasMaxLength(250);
                entity.Property(e => e.Address).IsRequired(false)
                .HasMaxLength(500);
                entity.Property(em => em.Email).IsRequired().HasMaxLength(250);


            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
