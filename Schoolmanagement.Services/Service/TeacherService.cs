using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Teacher;
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
    public class TeacherService : ITeacherService
    {
        private readonly IGenericRepository<Teacher> _teacherRepo;
        private readonly IMapper _mapper;

        public TeacherService(IGenericRepository<Teacher> teacherRepo, IMapper mapper)
        {
            _teacherRepo = teacherRepo;
            _mapper = mapper;
        }

        public void ClearChangeTracking() => _teacherRepo.ClearTracker();

        public async Task<IEnumerable<TeacherVm>> GetAllTeachersAsync()
        {
            var list = await _teacherRepo.GetQueryable().Where(x => !x.IsDeleted).ToListAsync();
            return _mapper.Map<IEnumerable<TeacherVm>>(list);
        }

        public async Task<TeacherVm?> GetTeacherByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _teacherRepo.GetQueryable().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            return _mapper.Map<TeacherVm>(entity);
        }

        public async Task<TeacherResponse> CreateTeacherAsync(TeacherRequest request, CancellationToken ct = default)
        {
            var response = new TeacherResponse();
            try
            {
                var entity = _mapper.Map<Teacher>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                await _teacherRepo.AddAsync(entity);
                await _teacherRepo.SaveChangesAsync();

                response.Teacher = _mapper.Map<TeacherVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<TeacherResponse> UpdateTeacherAsync(int id, TeacherRequest request, CancellationToken ct = default)
        {
            var response = new TeacherResponse();
            try
            {
                var entity = await _teacherRepo.GetQueryable().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
                if (entity == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Teacher with ID {id} not found.");
                    return response;
                }

                _mapper.Map(request, entity);
                entity.ModifiedDate = DateTime.UtcNow;

                _teacherRepo.Update(entity);
                await _teacherRepo.SaveChangesAsync();

                response.Teacher = _mapper.Map<TeacherVm>(entity);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<TeacherResponse> DeleteTeacherAsync(int id, CancellationToken ct = default)
        {
            var response = new TeacherResponse();
            try
            {
                var entity = await _teacherRepo.GetQueryable().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
                if (entity == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Teacher with ID {id} not found.");
                    return response;
                }

                entity.IsDeleted = true;
                entity.ModifiedDate = DateTime.UtcNow;

                _teacherRepo.Update(entity);
                await _teacherRepo.SaveChangesAsync();

                response.Teacher = _mapper.Map<TeacherVm>(entity);
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
