using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Role;
using Schoolmangenment.Entities.Model;
using Schoolmangenment.Repository.Data;
using Schoolmangenment.Repository.Interfaces;

namespace Schoolmanagement.Services.Service
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _repository;
        private readonly IMapper _mapper;

        public RoleService(IMapper mapper, IGenericRepository<Role> repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void ClearChangeTracking()
        {
            _repository.ClearTracker();
        }

        public void Dispose()
        {
        }

        public async Task<IEnumerable<RoleVm>> GetAllRolesAsync()
        {
            var roles = await _repository.GetQueryable()
                .Where(r => !r.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<RoleVm>>(roles);
        }

        public async Task<RoleVm?> GetRoleByIdAsync(int id, CancellationToken ct = default)
        {
            var role = await _repository.GetQueryable()
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
            return _mapper.Map<RoleVm>(role);
        }

        public async Task<RoleResponse> CreateRoleAsync(RoleRequest request, CancellationToken ct = default)
        {
            var response = new RoleResponse();
            using var transaction = await _repository.BeginTransactionAsync(ct);
            try
            {
                var role = new Role
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy,
                    IsActive = request.IsActive,
                    IsDeleted = false
                };

                await _repository.AddAsync(role);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync(ct);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.Role = _mapper.Map<RoleVm>(role);
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error creating role: " + ex.Message);
                return response;
            }
        }

        public async Task<RoleResponse> UpdateRoleAsync(int id, RoleRequest request, CancellationToken ct = default)
        {
            var response = new RoleResponse();
            try
            {
                var existing = await _repository.GetQueryable()
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);

                if (existing == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Role with ID {id} not found.");
                    return response;
                }

                existing.Name = request.Name;
                existing.Description = request.Description;
                existing.IsActive = request.IsActive;
                existing.ModifiedBy = request.ModifiedBy;
                existing.ModifiedDate = DateTime.UtcNow;

                _repository.Update(existing);
                await _repository.SaveChangesAsync();

                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.Role = _mapper.Map<RoleVm>(existing);
                return response;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error updating role: " + ex.Message);
                return response;
            }
        }

        public async Task<RoleResponse> DeleteRoleAsync(int id, CancellationToken ct = default)
        {
            var response = new RoleResponse();
            try
            {
                var role = await _repository.GetQueryable()
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);

                if (role == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Role with ID {id} not found.");
                    return response;
                }

                role.IsDeleted = true;
                role.IsActive = false;
                role.ModifiedDate = DateTime.UtcNow;

                _repository.Update(role);
                await _repository.SaveChangesAsync();

                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.Role = _mapper.Map<RoleVm>(role);
                return response;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error deleting role: " + ex.Message);
                return response;
            }
        }
    }
}
