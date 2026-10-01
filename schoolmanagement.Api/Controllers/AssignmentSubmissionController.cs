using Microsoft.AspNetCore.Mvc;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.ViewModel.AssignmentSubmission;
using Schoolmangenment.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace schoolmanagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentSubmissionController : ControllerBase
    {
        private readonly IAssignmentSubmissionService _submissionService;

        public AssignmentSubmissionController(IAssignmentSubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _submissionService.GetAllAssignmentSubmissionsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var item = await _submissionService.GetAssignmentSubmissionByIdAsync(id, ct);
            if (item == null)
            {
                return NotFound(new { message = $"Assignment Submission with ID {id} not found." });
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssignmentSubmissionRequest request, CancellationToken ct)
        {
            var response = await _submissionService.CreateAssignmentSubmissionAsync(request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.AssignmentSubmission?.Id }, response.AssignmentSubmission);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AssignmentSubmissionRequest request, CancellationToken ct)
        {
            var response = await _submissionService.UpdateAssignmentSubmissionAsync(id, request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.AssignmentSubmission);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _submissionService.DeleteAssignmentSubmissionAsync(id, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.AssignmentSubmission);
            }
            return BadRequest(response);
        }
    }
}
