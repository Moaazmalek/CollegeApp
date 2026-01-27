using CollegeApp.Data.Configuration;
using CollegeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDBContext(DbContextOptions options): DbContext(options)
    {

       public DbSet<Student> Students { get; set; }
       public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         modelBuilder.ApplyConfigurationsFromAssembly(typeof(CollegeDBContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
