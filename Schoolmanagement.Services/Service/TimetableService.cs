using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Timetable;
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
    public class TimetableService : ITimetableService
    {
        private readonly IGenericRepository<Timetable> _timetableRepo;
        private readonly IMapper _mapper;

        public TimetableService(IGenericRepository<Timetable> timetableRepo, IMapper mapper)
        {
            _timetableRepo = timetableRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _timetableRepo.ClearTracker();

        public async Task<IEnumerable<TimetableVm>> GetAllTimetablesAsync()
        {
            var list = await _timetableRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<TimetableVm>>(activeList);
        }

        public async Task<TimetableVm?> GetTimetableByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _timetableRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<TimetableVm>(entity);
        }

        public async Task<TimetableResponse> CreateTimetableAsync(TimetableRequest request, CancellationToken ct = default)
        {
            var response = new TimetableResponse();
            try
            {
                var entity = _mapper.Map<Timetable>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _timetableRepo.AddAsync(entity);
                await _timetableRepo.SaveChangesAsync();

                response.Timetable = _mapper.Map<TimetableVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<TimetableResponse> UpdateTimetableAsync(int id, TimetableRequest request, CancellationToken ct = default)
        {
            var response = new TimetableResponse();
            try
            {
                var entity = await _timetableRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Timetable with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _timetableRepo.Update(entity);
                await _timetableRepo.SaveChangesAsync();

                response.Timetable = _mapper.Map<TimetableVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<TimetableResponse> DeleteTimetableAsync(int id, CancellationToken ct = default)
        {
            var response = new TimetableResponse();
            try
            {
                var entity = await _timetableRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Timetable with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _timetableRepo.Update(entity);
                await _timetableRepo.SaveChangesAsync();

                response.Timetable = _mapper.Map<TimetableVm>(entity);
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
