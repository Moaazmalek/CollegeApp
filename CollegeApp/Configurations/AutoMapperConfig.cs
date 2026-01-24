using AutoMapper;
using CollegeApp.DTOs;
using CollegeApp.Models;
namespace CollegeApp.Configurations
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
        }

    }
}
