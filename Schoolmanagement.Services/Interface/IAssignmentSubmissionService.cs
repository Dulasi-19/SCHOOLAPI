using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.AssignmentSubmission;

namespace Schoolmanagement.Services.Interface
{
    public interface IAssignmentSubmissionService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<AssignmentSubmissionVm>> GetAllAssignmentSubmissionsAsync();
        Task<AssignmentSubmissionVm?> GetAssignmentSubmissionByIdAsync(int id, CancellationToken ct = default);
        Task<AssignmentSubmissionResponse> CreateAssignmentSubmissionAsync(AssignmentSubmissionRequest request, CancellationToken ct = default);
        Task<AssignmentSubmissionResponse> UpdateAssignmentSubmissionAsync(int id, AssignmentSubmissionRequest request, CancellationToken ct = default);
        Task<AssignmentSubmissionResponse> DeleteAssignmentSubmissionAsync(int id, CancellationToken ct = default);
    }
}
