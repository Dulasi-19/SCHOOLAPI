using Microsoft.AspNetCore.Mvc;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.ViewModel.Role;
using Schoolmangenment.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace schoolmanagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var role = await _roleService.GetRoleByIdAsync(id, ct);
            if (role == null)
            {
                return NotFound(new { message = $"Role with ID {id} not found." });
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleRequest request, CancellationToken ct)
        {
            var response = await _roleService.CreateRoleAsync(request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = response.Role?.Id }, response.Role);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleRequest request, CancellationToken ct)
        {
            var response = await _roleService.UpdateRoleAsync(id, request, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Role);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _roleService.DeleteRoleAsync(id, ct);
            if (response.RequestStatus == SchoolmanagementRequestStatus.NotFound)
            {
                return NotFound(response);
            }
            if (response.RequestStatus == SchoolmanagementRequestStatus.Success)
            {
                return Ok(response.Role);
            }
            return BadRequest(response);
        }
    }
}
