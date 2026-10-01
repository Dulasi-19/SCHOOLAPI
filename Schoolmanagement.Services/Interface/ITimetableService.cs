using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Timetable;

namespace Schoolmanagement.Services.Interface
{
    public interface ITimetableService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<TimetableVm>> GetAllTimetablesAsync();
        Task<TimetableVm?> GetTimetableByIdAsync(int id, CancellationToken ct = default);
        Task<TimetableResponse> CreateTimetableAsync(TimetableRequest request, CancellationToken ct = default);
        Task<TimetableResponse> UpdateTimetableAsync(int id, TimetableRequest request, CancellationToken ct = default);
        Task<TimetableResponse> DeleteTimetableAsync(int id, CancellationToken ct = default);
    }
}
