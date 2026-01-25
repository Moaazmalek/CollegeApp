using AutoMapper;
using AutoMapper.QueryableExtensions;
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.DTOs;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDBContext _context;
        private readonly IMapper _mapper;

        public StudentController(CollegeDBContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            //Good For Performance but Manual Mapping
            //var students = await _context.Students.Select(s => new StudentDTO()
            //{
            //    Name = s.Name,
            //    Email = s.Email,
            //    Address = s.Address,
            //    DOB = s.DOB

            //}).ToListAsync();

            //Using AutoMapper but bad for performance

            //var students = await _context.Students.ToListAsync();
            //var studentDTOData=_mapper.Map<List<StudentDTO>>(students);

            //Good for performance and AutoMapper
            var students = await _context.Students.ProjectTo<StudentDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
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
            
            var studentDto= _mapper.Map<StudentDTO>(student);
            return Ok(studentDto);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0) return BadRequest();

            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == model.Id);
            if (existingStudent == null) return NotFound();

            var newRecord = _mapper.Map<Student>(model);
            _context.Students.Update(newRecord);
           
            await _context.SaveChangesAsync();
            return model;
        }

        [HttpPatch("UpdatePartial/{id:int}")]
        public async Task<ActionResult<StudentDTO>> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument is null || id <= 0) return BadRequest();

            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStudent is null) return NotFound();

            var student = _mapper.Map<StudentDTO>(existingStudent);

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

            Student student =_mapper.Map<Student>(model);

            await  _context.Students.AddAsync(student);
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