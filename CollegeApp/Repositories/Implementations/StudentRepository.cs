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
    public class StudentRepository(CollegeDBContext context,IMapper mapper) : IStudentRepository
    {
        private readonly CollegeDBContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<StudentDTO> CreateStudentAsync(StudentDTO studentDto)
        {
            var student= _mapper.Map<Student>(studentDto);
            await _context.Students.AddAsync(student);  
            await _context.SaveChangesAsync();
            studentDto.Id=student.Id;
            return studentDto;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student=await  _context.Students.FirstOrDefaultAsync(s => s.Id ==id);
            if (student == null) return false; 
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            return await _context.Students.ProjectTo<StudentDTO>(_mapper.ConfigurationProvider).ToListAsync();
           
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var student= await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student==null ? null : student;  
        }

        public async Task<Student> GetStudentByNameAsync(string name)
        {
            var student=await _context.Students.FirstOrDefaultAsync(s => s.Name == name);
            return student==null ? null : student;
        }

        public async Task<StudentDTO> UpdateStudentAsync(StudentDTO studentDto)
        {
            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentDto.Id);
            if (existingStudent == null)
            {
                return null; // Student not found
            }
            _mapper.Map(studentDto, existingStudent);
            await _context.SaveChangesAsync();
            return _mapper.Map<StudentDTO>(existingStudent);
        }

        public async Task<StudentDTO> UpdateStudentPartialAsync(int id, JsonPatchDocument<StudentDTO> patchDoc)
        {
            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStudent == null) return null;

            var studentDto = _mapper.Map<StudentDTO>(existingStudent);

            patchDoc.ApplyTo(studentDto);

            // Apply patched values back to entity
            existingStudent.Name = studentDto.Name;
            existingStudent.Email = studentDto.Email;
            existingStudent.Address = studentDto.Address;
            existingStudent.DOB = studentDto.DOB;

            await _context.SaveChangesAsync();
            return studentDto;
        }
    }
}
