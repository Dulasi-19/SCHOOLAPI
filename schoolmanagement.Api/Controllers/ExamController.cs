using Microsoft.AspNetCore.Mvc;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.ViewModel.Exam;
using Schoolmangenment.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace schoolmanagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _examService.GetAllExamsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var item = await _examService.GetExamByIdAsync(id, ct);
            if (item == null)
            {
                return NotFound(new { message = $"Exam with ID {id} not found." });
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamRequest request, CancellationToken ct)
        {
            var response = await _examService.CreateExamAsync(request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Exam?.Id }, response.Exam);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExamRequest request, CancellationToken ct)
        {
            var response = await _examService.UpdateExamAsync(id, request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Exam);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _examService.DeleteExamAsync(id, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Exam);
            }
            return BadRequest(response);
        }
    }
}
