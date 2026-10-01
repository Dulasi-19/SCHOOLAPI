using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.AssignmentSubmission;
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
    public class AssignmentSubmissionService : IAssignmentSubmissionService
    {
        private readonly IGenericRepository<AssignmentSubmission> _submissionRepo;
        private readonly IMapper _mapper;

        public AssignmentSubmissionService(IGenericRepository<AssignmentSubmission> submissionRepo, IMapper mapper)
        {
            _submissionRepo = submissionRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _submissionRepo.ClearTracker();

        public async Task<IEnumerable<AssignmentSubmissionVm>> GetAllAssignmentSubmissionsAsync()
        {
            var list = await _submissionRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<AssignmentSubmissionVm>>(activeList);
        }

        public async Task<AssignmentSubmissionVm?> GetAssignmentSubmissionByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _submissionRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<AssignmentSubmissionVm>(entity);
        }

        public async Task<AssignmentSubmissionResponse> CreateAssignmentSubmissionAsync(AssignmentSubmissionRequest request, CancellationToken ct = default)
        {
            var response = new AssignmentSubmissionResponse();
            try
            {
                var entity = _mapper.Map<AssignmentSubmission>(request);
                entity.SubmissionDate = DateTime.UtcNow;
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _submissionRepo.AddAsync(entity);
                await _submissionRepo.SaveChangesAsync();

                response.AssignmentSubmission = _mapper.Map<AssignmentSubmissionVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<AssignmentSubmissionResponse> UpdateAssignmentSubmissionAsync(int id, AssignmentSubmissionRequest request, CancellationToken ct = default)
        {
            var response = new AssignmentSubmissionResponse();
            try
            {
                var entity = await _submissionRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Assignment Submission with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _submissionRepo.Update(entity);
                await _submissionRepo.SaveChangesAsync();

                response.AssignmentSubmission = _mapper.Map<AssignmentSubmissionVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<AssignmentSubmissionResponse> DeleteAssignmentSubmissionAsync(int id, CancellationToken ct = default)
        {
            var response = new AssignmentSubmissionResponse();
            try
            {
                var entity = await _submissionRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Assignment Submission with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _submissionRepo.Update(entity);
                await _submissionRepo.SaveChangesAsync();

                response.AssignmentSubmission = _mapper.Map<AssignmentSubmissionVm>(entity);
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
