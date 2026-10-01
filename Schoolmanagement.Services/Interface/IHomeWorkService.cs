using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.HomeWork;

namespace Schoolmanagement.Services.Interface
{
    public interface IHomeWorkService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<HomeWorkVm>> GetAllHomeWorksAsync();
        Task<HomeWorkVm?> GetHomeWorkByIdAsync(int id, CancellationToken ct = default);
        Task<HomeWorkResponse> CreateHomeWorkAsync(HomeWorkRequest request, CancellationToken ct = default);
        Task<HomeWorkResponse> UpdateHomeWorkAsync(int id, HomeWorkRequest request, CancellationToken ct = default);
        Task<HomeWorkResponse> DeleteHomeWorkAsync(int id, CancellationToken ct = default);
    }
}