using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Assignment;

namespace Schoolmanagement.Services.Interface
{
    public interface IAssignmentService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<AssignmentVm>> GetAllAssignmentsAsync();
        Task<AssignmentVm?> GetAssignmentByIdAsync(int id, CancellationToken ct = default);
        Task<AssignmentResponse> CreateAssignmentAsync(AssignmentRequest request, CancellationToken ct = default);
        Task<AssignmentResponse> UpdateAssignmentAsync(int id, AssignmentRequest request, CancellationToken ct = default);
        Task<AssignmentResponse> DeleteAssignmentAsync(int id, CancellationToken ct = default);
    }
}
