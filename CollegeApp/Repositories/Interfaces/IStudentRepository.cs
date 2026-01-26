using Azure;
using CollegeApp.DTOs;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace CollegeApp.Repositories.Interfaces
{
    public interface IStudentRepository:IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsByFeeStatusAsync(int feeStatus);



    }
}
