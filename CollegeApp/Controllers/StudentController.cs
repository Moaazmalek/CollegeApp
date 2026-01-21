using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;
using CollegeApp.Repository;

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
        [HttpGet]
        public Student GetStudentByName(string name)
        {

            return CollegeRepository.Students.FirstOrDefault(s => s.Name == name);
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