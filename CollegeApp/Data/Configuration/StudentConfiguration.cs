using CollegeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CollegeApp.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).UseIdentityColumn();
            builder.Property(n => n.Name).IsRequired()
            .HasMaxLength(250);
            builder.Property(e => e.Address).IsRequired(false)
            .HasMaxLength(500);
            builder.Property(em => em.Email).IsRequired().HasMaxLength(250);
            builder.HasData(
             [
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
                ]
              );
        }
    }
}
