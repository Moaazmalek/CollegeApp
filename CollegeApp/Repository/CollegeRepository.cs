using CollegeApp.Models;

namespace CollegeApp.Repository
{
    public  static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = [
                            new Student { Id = 1, Name = "Alice Johnson", Email = "Alice@gmail.com", Address = "123 Main St, Cityville" },
            new(){ Id = 2, Name = "Bob Smith", Email = "BoB@gmail.com", Address = "456 Oak Ave, Townsville" },

            ];
    }
}
