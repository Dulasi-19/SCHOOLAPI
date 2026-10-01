using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Class;

namespace Schoolmanagement.Services.Interface
{
    public interface IClassService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<ClassVm>> GetAllClassesAsync();
        Task<ClassVm?> GetClassByIdAsync(int id, CancellationToken ct = default);
        Task<ClassResponse> CreateClassAsync(ClassRequest request, CancellationToken ct = default);
        Task<ClassResponse> UpdateClassAsync(int id, ClassRequest request, CancellationToken ct = default);
        Task<ClassResponse> DeleteClassAsync(int id, CancellationToken ct = default);
    }
}
