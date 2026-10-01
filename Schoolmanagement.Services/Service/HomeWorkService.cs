using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.HomeWork;
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
    public class HomeWorkService : IHomeWorkService
    {
        private readonly IGenericRepository<HomeWork> _homeWorkRepo;
        private readonly IMapper _mapper;

        public HomeWorkService(IGenericRepository<HomeWork> homeWorkRepo, IMapper mapper)
        {
            _homeWorkRepo = homeWorkRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _homeWorkRepo.ClearTracker();

        public async Task<IEnumerable<HomeWorkVm>> GetAllHomeWorksAsync()
        {
            var list = await _homeWorkRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<HomeWorkVm>>(activeList);
        }

        public async Task<HomeWorkVm?> GetHomeWorkByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _homeWorkRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<HomeWorkVm>(entity);
        }

        public async Task<HomeWorkResponse> CreateHomeWorkAsync(HomeWorkRequest request, CancellationToken ct = default)
        {
            var response = new HomeWorkResponse();
            try
            {
                var entity = _mapper.Map<HomeWork>(request);
                entity.AssignDate = DateTime.UtcNow;
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _homeWorkRepo.AddAsync(entity);
                await _homeWorkRepo.SaveChangesAsync();

                response.HomeWork = _mapper.Map<HomeWorkVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<HomeWorkResponse> UpdateHomeWorkAsync(int id, HomeWorkRequest request, CancellationToken ct = default)
        {
            var response = new HomeWorkResponse();
            try
            {
                var entity = await _homeWorkRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"HomeWork with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _homeWorkRepo.Update(entity);
                await _homeWorkRepo.SaveChangesAsync();

                response.HomeWork = _mapper.Map<HomeWorkVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<HomeWorkResponse> DeleteHomeWorkAsync(int id, CancellationToken ct = default)
        {
            var response = new HomeWorkResponse();
            try
            {
                var entity = await _homeWorkRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"HomeWork with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _homeWorkRepo.Update(entity);
                await _homeWorkRepo.SaveChangesAsync();

                response.HomeWork = _mapper.Map<HomeWorkVm>(entity);
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