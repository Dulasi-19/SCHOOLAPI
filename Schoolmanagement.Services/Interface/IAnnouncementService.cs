using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Announcement;

namespace Schoolmanagement.Services.Interface
{
    public interface IAnnouncementService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<AnnouncementVm>> GetAllAnnouncementsAsync();
        Task<AnnouncementVm?> GetAnnouncementByIdAsync(int id, CancellationToken ct = default);
        Task<AnnouncementResponse> CreateAnnouncementAsync(AnnouncementRequest request, CancellationToken ct = default);
        Task<AnnouncementResponse> UpdateAnnouncementAsync(int id, AnnouncementRequest request, CancellationToken ct = default);
        Task<AnnouncementResponse> DeleteAnnouncementAsync(int id, CancellationToken ct = default);
    }
}
