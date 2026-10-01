using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Notification;

namespace Schoolmanagement.Services.Interface
{
    public interface INotificationService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<NotificationVm>> GetAllNotificationsAsync();
        Task<NotificationVm?> GetNotificationByIdAsync(int id, CancellationToken ct = default);
        Task<NotificationResponse> CreateNotificationAsync(NotificationRequest request, CancellationToken ct = default);
        Task<NotificationResponse> UpdateNotificationAsync(int id, NotificationRequest request, CancellationToken ct = default);
        Task<NotificationResponse> DeleteNotificationAsync(int id, CancellationToken ct = default);
    }
}
