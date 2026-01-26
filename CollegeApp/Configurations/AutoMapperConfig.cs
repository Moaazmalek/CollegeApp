using AutoMapper;
using CollegeApp.DTOs;
using CollegeApp.Models;
namespace CollegeApp.Configurations
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            //Add Transfromation rules here
            CreateMap<Student, StudentDTO>().ReverseMap().ForMember( d => d.Address,opt => opt.NullSubstitute("Address not found"));
        }

    }
}
