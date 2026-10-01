using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Role;

namespace Schoolmanagement.Services.Interface
{
    public interface IRoleService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<RoleVm>> GetAllRolesAsync();
        Task<RoleVm?> GetRoleByIdAsync(int id, CancellationToken ct = default);
        Task<RoleResponse> CreateRoleAsync(RoleRequest request, CancellationToken ct = default);
        Task<RoleResponse> UpdateRoleAsync(int id, RoleRequest request, CancellationToken ct = default);
        Task<RoleResponse> DeleteRoleAsync(int id, CancellationToken ct = default);
    }
}
