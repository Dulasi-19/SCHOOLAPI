using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Announcement;
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
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IGenericRepository<Announcement> _announcementRepo;
        private readonly IMapper _mapper;

        public AnnouncementService(IGenericRepository<Announcement> announcementRepo, IMapper mapper)
        {
            _announcementRepo = announcementRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _announcementRepo.ClearTracker();

        public async Task<IEnumerable<AnnouncementVm>> GetAllAnnouncementsAsync()
        {
            var list = await _announcementRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<AnnouncementVm>>(activeList);
        }

        public async Task<AnnouncementVm?> GetAnnouncementByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _announcementRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<AnnouncementVm>(entity);
        }

        public async Task<AnnouncementResponse> CreateAnnouncementAsync(AnnouncementRequest request, CancellationToken ct = default)
        {
            var response = new AnnouncementResponse();
            try
            {
                var entity = _mapper.Map<Announcement>(request);
                entity.PostedDate = DateTime.UtcNow;
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _announcementRepo.AddAsync(entity);
                await _announcementRepo.SaveChangesAsync();

                response.Announcement = _mapper.Map<AnnouncementVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<AnnouncementResponse> UpdateAnnouncementAsync(int id, AnnouncementRequest request, CancellationToken ct = default)
        {
            var response = new AnnouncementResponse();
            try
            {
                var entity = await _announcementRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Announcement with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _announcementRepo.Update(entity);
                await _announcementRepo.SaveChangesAsync();

                response.Announcement = _mapper.Map<AnnouncementVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<AnnouncementResponse> DeleteAnnouncementAsync(int id, CancellationToken ct = default)
        {
            var response = new AnnouncementResponse();
            try
            {
                var entity = await _announcementRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Announcement with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _announcementRepo.Update(entity);
                await _announcementRepo.SaveChangesAsync();

                response.Announcement = _mapper.Map<AnnouncementVm>(entity);
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
