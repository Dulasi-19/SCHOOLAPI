using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.User;

namespace Schoolmanagement.Services.Interface
{
    public interface IUserService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<UserVm>> GetAllUsersAsync();
        Task<UserVm?> GetUserByIdAsync(int id, CancellationToken ct = default);
        Task<UserResponse> CreateUserAsync(UserRequest request, CancellationToken ct = default);
        Task<UserResponse> UpdateUserAsync(int id, UserRequest request, CancellationToken ct = default);
        Task<UserResponse> DeleteUserAsync(int id, CancellationToken ct = default);
    }
}
