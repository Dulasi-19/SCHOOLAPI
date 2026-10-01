using Microsoft.AspNetCore.Mvc;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.ViewModel.Teacher;
using Schoolmangenment.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace schoolmanagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id, ct);
            if (teacher == null)
            {
                return NotFound(new { message = $"Teacher with ID {id} not found." });
            }
            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherRequest request, CancellationToken ct)
        {
            var response = await _teacherService.CreateTeacherAsync(request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Teacher?.Id }, response.Teacher);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeacherRequest request, CancellationToken ct)
        {
            var response = await _teacherService.UpdateTeacherAsync(id, request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Teacher);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _teacherService.DeleteTeacherAsync(id, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Teacher);
            }
            return BadRequest(response);
        }
    }
}
