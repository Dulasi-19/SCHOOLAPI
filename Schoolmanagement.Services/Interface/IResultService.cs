using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Result;

namespace Schoolmanagement.Services.Interface
{
    public interface IResultService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<ResultVm>> GetAllResultsAsync();
        Task<ResultVm?> GetResultByIdAsync(int id, CancellationToken ct = default);
        Task<ResultResponse> CreateResultAsync(ResultRequest request, CancellationToken ct = default);
        Task<ResultResponse> UpdateResultAsync(int id, ResultRequest request, CancellationToken ct = default);
        Task<ResultResponse> DeleteResultAsync(int id, CancellationToken ct = default);
    }
}
