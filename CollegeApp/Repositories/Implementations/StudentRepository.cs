using AutoMapper;
using AutoMapper.QueryableExtensions;
using CollegeApp.Data;
using CollegeApp.DTOs;
using CollegeApp.Models;
using CollegeApp.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Repositories.Implementations
{
    public class StudentRepository(CollegeDBContext context) : GenericRepository<Student>(context) , IStudentRepository
    {
        private readonly CollegeDBContext _context = context;

        public Task<IEnumerable<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
        {
            throw new NotImplementedException();
        }
    }
}
