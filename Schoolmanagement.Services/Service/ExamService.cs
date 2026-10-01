using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Exam;
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
    public class ExamService : IExamService
    {
        private readonly IGenericRepository<Exam> _examRepo;
        private readonly IMapper _mapper;

        public ExamService(IGenericRepository<Exam> examRepo, IMapper mapper)
        {
            _examRepo = examRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _examRepo.ClearTracker();

        public async Task<IEnumerable<ExamVm>> GetAllExamsAsync()
        {
            var list = await _examRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<ExamVm>>(activeList);
        }

        public async Task<ExamVm?> GetExamByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _examRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<ExamVm>(entity);
        }

        public async Task<ExamResponse> CreateExamAsync(ExamRequest request, CancellationToken ct = default)
        {
            var response = new ExamResponse();
            try
            {
                var entity = _mapper.Map<Exam>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _examRepo.AddAsync(entity);
                await _examRepo.SaveChangesAsync();

                response.Exam = _mapper.Map<ExamVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<ExamResponse> UpdateExamAsync(int id, ExamRequest request, CancellationToken ct = default)
        {
            var response = new ExamResponse();
            try
            {
                var entity = await _examRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Exam with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _examRepo.Update(entity);
                await _examRepo.SaveChangesAsync();

                response.Exam = _mapper.Map<ExamVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<ExamResponse> DeleteExamAsync(int id, CancellationToken ct = default)
        {
            var response = new ExamResponse();
            try
            {
                var entity = await _examRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Exam with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _examRepo.Update(entity);
                await _examRepo.SaveChangesAsync();

                response.Exam = _mapper.Map<ExamVm>(entity);
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
