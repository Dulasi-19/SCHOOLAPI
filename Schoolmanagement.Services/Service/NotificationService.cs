using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Notification;
using Schoolmangenment.Entities.Model;
using Schoolmangenment.Repository.Data;
using Schoolmangenment.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Schoolmanagement.Services.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepo;
        private readonly IMapper _mapper;

        public NotificationService(IGenericRepository<Notification> notificationRepo, IMapper mapper)
        {
            _notificationRepo = notificationRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _notificationRepo.ClearTracker();

        public async Task<IEnumerable<NotificationVm>> GetAllNotificationsAsync()
        {
            var list = await _notificationRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<NotificationVm>>(activeList);
        }

        public async Task<NotificationVm?> GetNotificationByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _notificationRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<NotificationVm>(entity);
        }

        public async Task<NotificationResponse> CreateNotificationAsync(NotificationRequest request, CancellationToken ct = default)
        {
            var response = new NotificationResponse();
            try
            {
                var entity = _mapper.Map<Notification>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _notificationRepo.AddAsync(entity);
                await _notificationRepo.SaveChangesAsync();

                response.Notification = _mapper.Map<NotificationVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<NotificationResponse> UpdateNotificationAsync(int id, NotificationRequest request, CancellationToken ct = default)
        {
            var response = new NotificationResponse();
            try
            {
                var entity = await _notificationRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Notification with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _notificationRepo.Update(entity);
                await _notificationRepo.SaveChangesAsync();

                response.Notification = _mapper.Map<NotificationVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<NotificationResponse> DeleteNotificationAsync(int id, CancellationToken ct = default)
        {
            var response = new NotificationResponse();
            try
            {
                var entity = await _notificationRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Notification with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _notificationRepo.Update(entity);
                await _notificationRepo.SaveChangesAsync();

                response.Notification = _mapper.Map<NotificationVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
