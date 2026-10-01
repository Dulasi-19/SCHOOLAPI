using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Teacher;

namespace Schoolmanagement.Services.Interface
{
    public interface ITeacherService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<TeacherVm>> GetAllTeachersAsync();
        Task<TeacherVm?> GetTeacherByIdAsync(int id, CancellationToken ct = default);
        Task<TeacherResponse> CreateTeacherAsync(TeacherRequest request, CancellationToken ct = default);
        Task<TeacherResponse> UpdateTeacherAsync(int id, TeacherRequest request, CancellationToken ct = default);
        Task<TeacherResponse> DeleteTeacherAsync(int id, CancellationToken ct = default);
    }
}
