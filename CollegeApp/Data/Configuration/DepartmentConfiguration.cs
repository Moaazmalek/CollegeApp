using CollegeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Configuration
{
    public class DepartmentConfiguration: IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).UseIdentityColumn();
            builder.Property(n => n.DepartmentName).IsRequired()
            .HasMaxLength(250);
            builder.Property(e => e.DepartmentDescription).IsRequired(false)
            .HasMaxLength(500);
            builder.HasData(
             [
                 new
              {
                  Id = 1,
                  DepartmentName = "ECE",
                  DepartmentDescription = "ECE Department"
              },
             
                 new
              {
                  Id = 2,
                  DepartmentName = "CSE",
                  DepartmentDescription = "CSE Department"
              }
             
                ]
              );

            // add configuration for Foreign keys
            // one department have many Students
            //builder.HasMany(s => s.Students)
            //       .WithOne(d => d.Department)
            //       .HasForeignKey(d => d.DepartmentId)
            //       .HasConstraintName("FK_Students_Departments")
            //       .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
