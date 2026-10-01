using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Class;
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
    public class ClassService : IClassService
    {
        private readonly IGenericRepository<SchoolClass> _classRepo;
        private readonly IMapper _mapper;

        public ClassService(IGenericRepository<SchoolClass> classRepo, IMapper mapper)
        {
            _classRepo = classRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _classRepo.ClearTracker();

        public async Task<IEnumerable<ClassVm>> GetAllClassesAsync()
        {
            var list = await _classRepo.GetQueryable().Where(x => !x.IsDeleted).ToListAsync();
            return _mapper.Map<IEnumerable<ClassVm>>(list);
        }

        public async Task<ClassVm?> GetClassByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _classRepo.GetQueryable().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            return _mapper.Map<ClassVm>(entity);
        }

        public async Task<ClassResponse> CreateClassAsync(ClassRequest request, CancellationToken ct = default)
        {
            var response = new ClassResponse();
            try
            {
                var entity = _mapper.Map<SchoolClass>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _classRepo.AddAsync(entity);
                await _classRepo.SaveChangesAsync();

                response.Class = _mapper.Map<ClassVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<ClassResponse> UpdateClassAsync(int id, ClassRequest request, CancellationToken ct = default)
        {
            var response = new ClassResponse();
            try
            {
                var entity = await _classRepo.GetQueryable().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
                if (entity == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Class with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _classRepo.Update(entity);
                await _classRepo.SaveChangesAsync();

                response.Class = _mapper.Map<ClassVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<ClassResponse> DeleteClassAsync(int id, CancellationToken ct = default)
        {
            var response = new ClassResponse();
            try
            {
                var entity = await _classRepo.GetQueryable().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
                if (entity == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Class with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _classRepo.Update(entity);
                await _classRepo.SaveChangesAsync();

                response.Class = _mapper.Map<ClassVm>(entity);
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
