using AutoMapper;
using CollegeApp.DTOs;
using CollegeApp.Models;
using CollegeApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOnlyLocalhost")]
    [Authorize(AuthenticationSchemes ="LoginForGoogleUsers",Roles ="Superadmin,Admin")]
    public class StudentController : ControllerBase
    {
      
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Student> _repository;
        private readonly IStudentRepository _studentRepository;
        private APIResponse _apiResponse;

        public StudentController( IMapper mapper,IGenericRepository<Student> repository,IStudentRepository studentRepository)
        { 
            _mapper = mapper;
            _repository = repository;
            _studentRepository = studentRepository;
            _apiResponse = new APIResponse();
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _repository.GetAllAsync();
            _apiResponse.Data=_mapper.Map<IEnumerable<StudentDTO>>(students);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);

        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null) return NotFound();
            
            var studentDTO=_mapper.Map<StudentDTO>(student);
            _apiResponse.Data=studentDTO;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet("{name:alpha}",Name ="GetStudentByName")]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if(string.IsNullOrEmpty(name)) return BadRequest();
            var student =await _repository.GetAsync(s => s.Name== name);    
            if(student == null) return NotFound();
            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO model)
        {
            
            if (model == null || model.Id <= 0) return BadRequest();
            var student=await _repository.GetByIdAsync(model.Id);
            if (student == null) return NotFound();
            _mapper.Map(model, student);
            await _repository.UpdateAsync(student);
            return Ok(_mapper.Map<StudentDTO>(student));

        }

        //[HttpPatch("UpdatePartial/{id:int}")]
        //public async Task<ActionResult<StudentDTO>> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        //{
        //    if (patchDocument == null || id <= 0) return BadRequest();

        //    var updatedStudent=await _studentRepository.UpdateStudentPartialAsync(id, patchDocument);
        //    if (updatedStudent == null) return NotFound();
        //    return Ok(updatedStudent);
        //}

        [HttpPost("Create")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO model)
        {
            if (model is null) return BadRequest();

             var student=_mapper.Map<Student>(model);
            await _repository.CreateAsync(student);
            return CreatedAtRoute(
                "GetStudentById",
                new { id = student.Id },
                _mapper.Map<StudentDTO>(student));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);
            if (isDeleted == false) return NotFound();
            return NoContent();
        }
    }
}