using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Result;
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
    public class ResultService : IResultService
    {
        private readonly IGenericRepository<Result> _resultRepo;
        private readonly IMapper _mapper;

        public ResultService(IGenericRepository<Result> resultRepo, IMapper mapper)
        {
            _resultRepo = resultRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _resultRepo.ClearTracker();

        public async Task<IEnumerable<ResultVm>> GetAllResultsAsync()
        {
            var list = await _resultRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<ResultVm>>(activeList);
        }

        public async Task<ResultVm?> GetResultByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _resultRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<ResultVm>(entity);
        }

        public async Task<ResultResponse> CreateResultAsync(ResultRequest request, CancellationToken ct = default)
        {
            var response = new ResultResponse();
            try
            {
                var entity = _mapper.Map<Result>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _resultRepo.AddAsync(entity);
                await _resultRepo.SaveChangesAsync();

                response.Result = _mapper.Map<ResultVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<ResultResponse> UpdateResultAsync(int id, ResultRequest request, CancellationToken ct = default)
        {
            var response = new ResultResponse();
            try
            {
                var entity = await _resultRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Result with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _resultRepo.Update(entity);
                await _resultRepo.SaveChangesAsync();

                response.Result = _mapper.Map<ResultVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<ResultResponse> DeleteResultAsync(int id, CancellationToken ct = default)
        {
            var response = new ResultResponse();
            try
            {
                var entity = await _resultRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Result with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _resultRepo.Update(entity);
                await _resultRepo.SaveChangesAsync();

                response.Result = _mapper.Map<ResultVm>(entity);
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
