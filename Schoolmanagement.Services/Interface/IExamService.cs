using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Exam;

namespace Schoolmanagement.Services.Interface
{
    public interface IExamService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<ExamVm>> GetAllExamsAsync();
        Task<ExamVm?> GetExamByIdAsync(int id, CancellationToken ct = default);
        Task<ExamResponse> CreateExamAsync(ExamRequest request, CancellationToken ct = default);
        Task<ExamResponse> UpdateExamAsync(int id, ExamRequest request, CancellationToken ct = default);
        Task<ExamResponse> DeleteExamAsync(int id, CancellationToken ct = default);
    }
}
