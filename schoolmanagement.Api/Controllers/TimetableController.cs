using Microsoft.AspNetCore.Mvc;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.ViewModel.Timetable;
using Schoolmangenment.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace schoolmanagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly ITimetableService _timetableService;

        public TimetableController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _timetableService.GetAllTimetablesAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var item = await _timetableService.GetTimetableByIdAsync(id, ct);
            if (item == null)
            {
                return NotFound(new { message = $"Timetable with ID {id} not found." });
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TimetableRequest request, CancellationToken ct)
        {
            var response = await _timetableService.CreateTimetableAsync(request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Timetable?.Id }, response.Timetable);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TimetableRequest request, CancellationToken ct)
        {
            var response = await _timetableService.UpdateTimetableAsync(id, request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Timetable);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _timetableService.DeleteTimetableAsync(id, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Timetable);
            }
            return BadRequest(response);
        }
    }
}
