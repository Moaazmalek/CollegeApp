using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;
using CollegeApp.Repository;
using CollegeApp.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace CollegeApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All")]
        public IEnumerable<Student> GetStudents()
        {
            return CollegeRepository.Students;
        }
        [HttpGet("{id:int}")]
        public Student GetStudentById(int id)
        {

            return CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
        }
        [HttpGet("{name:alpha}")]
        public Student GetStudentByName(string name)
        {

            return CollegeRepository.Students.FirstOrDefault(s => s.Name == name);
        }
        [HttpPut("Update")]
        public ActionResult<StudentDTO> UpdateStudent( [FromBody] StudentDTO model)
        {
            if (model == null || model.Id <=0)
            {
                return BadRequest();
            }
            var existingStudent = CollegeRepository.Students.FirstOrDefault(s => s.Id == model.Id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.Name = model.Name;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;
            return model;

        }
        [HttpPatch("UpdatePartial/{id:int}")]
        public ActionResult<StudentDTO> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument )
        {
            if (patchDocument is null || id <=0) return BadRequest();
            var existingStudent = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
            if (existingStudent is null) return NotFound();

            var student= new StudentDTO
            {
                Id = existingStudent.Id,
                Name = existingStudent.Name,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };

            patchDocument.ApplyTo(student, ModelState);

            if(!ModelState.IsValid) return BadRequest(ModelState);
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Address = student.Address;  
            return student;




        }

        [HttpPost("Create")]
        public IActionResult CreateStudent([FromBody]StudentDTO model)
        {
            if (model is null) return BadRequest();
            int newId= CollegeRepository.Students.Max(s => s.Id) + 1;
           
            Student student = new()
            {
                Id = newId,
                Name = model.Name,
                Email = model.Email,
                Address = model.Address

            };

            CollegeRepository.Students.Add(student);
            model.Id = newId;
            return CreatedAtRoute(
                routeName: nameof(GetStudentById),
                routeValues: new { id = student.Id },
                value: model
                );

        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = CollegeRepository.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            CollegeRepository.Students.Remove(student);
            return NoContent();




        }

    }
}