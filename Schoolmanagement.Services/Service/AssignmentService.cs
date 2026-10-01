using AutoMapper;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Assignment;
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
    public class AssignmentService : IAssignmentService
    {
        private readonly IGenericRepository<Assignment> _assignmentRepo;
        private readonly IMapper _mapper;

        public AssignmentService(IGenericRepository<Assignment> assignmentRepo, IMapper mapper)
        {
            _assignmentRepo = assignmentRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _assignmentRepo.ClearTracker();

        public async Task<IEnumerable<AssignmentVm>> GetAllAssignmentsAsync()
        {
            var list = await _assignmentRepo.GetAllAsync();
            var activeList = list.Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<AssignmentVm>>(activeList);
        }

        public async Task<AssignmentVm?> GetAssignmentByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _assignmentRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;
            return _mapper.Map<AssignmentVm>(entity);
        }

        public async Task<AssignmentResponse> CreateAssignmentAsync(AssignmentRequest request, CancellationToken ct = default)
        {
            var response = new AssignmentResponse();
            try
            {
                var entity = _mapper.Map<Assignment>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _assignmentRepo.AddAsync(entity);
                await _assignmentRepo.SaveChangesAsync();

                response.Assignment = _mapper.Map<AssignmentVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<AssignmentResponse> UpdateAssignmentAsync(int id, AssignmentRequest request, CancellationToken ct = default)
        {
            var response = new AssignmentResponse();
            try
            {
                var entity = await _assignmentRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Assignment with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _assignmentRepo.Update(entity);
                await _assignmentRepo.SaveChangesAsync();

                response.Assignment = _mapper.Map<AssignmentVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<AssignmentResponse> DeleteAssignmentAsync(int id, CancellationToken ct = default)
        {
            var response = new AssignmentResponse();
            try
            {
                var entity = await _assignmentRepo.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Assignment with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _assignmentRepo.Update(entity);
                await _assignmentRepo.SaveChangesAsync();

                response.Assignment = _mapper.Map<AssignmentVm>(entity);
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
