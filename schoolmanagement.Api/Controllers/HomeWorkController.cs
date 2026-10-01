using Microsoft.AspNetCore.Mvc;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.ViewModel.HomeWork;
using Schoolmangenment.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace schoolmanagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeWorkController : ControllerBase
    {
        private readonly IHomeWorkService _homeWorkService;

        public HomeWorkController(IHomeWorkService homeWorkService)
        {
            _homeWorkService = homeWorkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _homeWorkService.GetAllHomeWorksAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var item = await _homeWorkService.GetHomeWorkByIdAsync(id, ct);
            if (item == null)
            {
                return NotFound(new { message = $"HomeWork with ID {id} not found." });
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HomeWorkRequest request, CancellationToken ct)
        {
            var response = await _homeWorkService.CreateHomeWorkAsync(request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.HomeWork?.Id }, response.HomeWork);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HomeWorkRequest request, CancellationToken ct)
        {
            var response = await _homeWorkService.UpdateHomeWorkAsync(id, request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.HomeWork);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _homeWorkService.DeleteHomeWorkAsync(id, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.HomeWork);
            }
            return BadRequest(response);
        }
    }
}
