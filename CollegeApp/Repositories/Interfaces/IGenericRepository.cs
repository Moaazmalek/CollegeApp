using CollegeApp.DTOs;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace CollegeApp.Repositories.Interfaces
{
    public interface IGenericRepository<T>
    {
        // Define method signatures for student data operations
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetAsync(Expression<Func<T,bool>> predicate);
        Task<T> UpdateAsync(T dbRecord);
        //Task<T> UpdateStudentPartialAsync(int id, JsonPatchDocument<StudentDTO> patchDoc);
        Task<T> CreateAsync(T dbRecord);
        Task<bool> DeleteAsync(int id);
    }
}
