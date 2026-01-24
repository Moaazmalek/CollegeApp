using CollegeApp.Data;
using CollegeApp.DTOs;
using CollegeApp.Models;
using CollegeApp.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDBContext _context;

        public StudentController(CollegeDBContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students.Select(s => new StudentDTO()
            {
                Name = s.Name,
                Email = s.Email,
                Address = s.Address,
                DOB = s.DOB

            }).ToListAsync();
            return Ok(students);

        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return student;
        }

        [HttpGet("{name:alpha}",Name ="GetStudentByName")]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if(string.IsNullOrEmpty(name)) return BadRequest();
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Name == name);
            if (student == null) return NotFound();
            var studentDto = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB
            };
            return Ok(studentDto);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0) return BadRequest();

            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == model.Id);
            if (existingStudent == null) return NotFound();

            var newRecord = new Student()
            {
                Id = existingStudent.Id,
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
                DOB = model.DOB
            };
            _context.Students.Update(newRecord);
            //existingStudent.Name = model.Name;
            //existingStudent.Email = model.Email;
            //existingStudent.Address = model.Address;
            //existingStudent.DOB = model.DOB;

            await _context.SaveChangesAsync();
            return model;
        }

        [HttpPatch("UpdatePartial/{id:int}")]
        public async Task<ActionResult<StudentDTO>> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument is null || id <= 0) return BadRequest();

            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStudent is null) return NotFound();

            var student = new StudentDTO
            {
                Id = existingStudent.Id,
                Name = existingStudent.Name,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };

            patchDocument.ApplyTo(student, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Address = student.Address;

            await _context.SaveChangesAsync();
            return student;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO model)
        {
            if (model is null) return BadRequest();

            Student student = new()
            {
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
                DOB = model.DOB
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            model.Id = student.Id; // return the generated Id

            return CreatedAtRoute("GetStudentById", new { id = student.Id }, model);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}