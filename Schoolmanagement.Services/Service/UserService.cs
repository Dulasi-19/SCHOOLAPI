using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.User;
using Schoolmangenment.Entities.Model;
using Schoolmangenment.Repository.Data;
using Schoolmangenment.Repository.Interfaces;

namespace Schoolmanagement.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IGenericRepository<User> repository)
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

        public async Task<IEnumerable<UserVm>> GetAllUsersAsync()
        {
            var users = await _repository.GetQueryable()
                .Where(u => !u.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserVm>>(users);
        }

        public async Task<UserVm?> GetUserByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _repository.GetQueryable()
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, ct);
            return _mapper.Map<UserVm>(user);
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest request, CancellationToken ct = default)
        {
            var response = new UserResponse();

            var existing = await _repository.GetQueryable()
                .FirstOrDefaultAsync(u => u.Username == request.Username && !u.IsDeleted, ct);

            if (existing != null)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.ValidationError;
                response.AddError($"Username '{request.Username}' already exists.");
                return response;
            }

            using var transaction = await _repository.BeginTransactionAsync(ct);
            try
            {
                var dbUser = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.PasswordHash),
                    Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role,
                    RoleId = (request.RoleId.HasValue && request.RoleId.Value > 0) ? request.RoleId : null,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy,
                    IsActive = request.IsActive,
                    IsDeleted = false
                };

                await _repository.AddAsync(dbUser);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync(ct);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.User = _mapper.Map<UserVm>(dbUser);
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                var errorMsg = ex.InnerException != null ? $"{ex.Message} -> {ex.InnerException.Message}" : ex.Message;
                response.AddError("Error creating user: " + errorMsg);
                return response;
            }
        }

        public async Task<UserResponse> UpdateUserAsync(int id, UserRequest request, CancellationToken ct = default)
        {
            var response = new UserResponse();
            try
            {
                var existingUser = await _repository.GetQueryable()
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, ct);

                if (existingUser == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"User with ID {id} not found.");
                    return response;
                }

                var duplicate = await _repository.GetQueryable()
                    .AnyAsync(u => u.Id != id && u.Username == request.Username && !u.IsDeleted, ct);

                if (duplicate)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.ValidationError;
                    response.AddError($"Username '{request.Username}' is already taken.");
                    return response;
                }

                existingUser.Username = request.Username;
                existingUser.Email = request.Email;
                existingUser.Role = string.IsNullOrWhiteSpace(request.Role) ? existingUser.Role : request.Role;
                existingUser.RoleId = (request.RoleId.HasValue && request.RoleId.Value > 0) ? request.RoleId : null;
                existingUser.IsActive = request.IsActive;
                existingUser.IsDeleted = request.IsDeleted;
                existingUser.ModifiedBy = request.ModifiedBy;
                existingUser.ModifiedDate = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(request.PasswordHash) && request.PasswordHash != existingUser.PasswordHash)
                {
                    existingUser.PasswordHash = HashPassword(request.PasswordHash);
                }

                _repository.Update(existingUser);
                await _repository.SaveChangesAsync();

                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.User = _mapper.Map<UserVm>(existingUser);
                return response;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error updating user: " + ex.Message);
                return response;
            }
        }

        public async Task<UserResponse> DeleteUserAsync(int id, CancellationToken ct = default)
        {
            var response = new UserResponse();
            try
            {
                var user = await _repository.GetQueryable()
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, ct);

                if (user == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"User with ID {id} not found.");
                    return response;
                }

                user.IsDeleted = true;
                user.IsActive = false;
                user.ModifiedDate = DateTime.UtcNow;

                _repository.Update(user);
                await _repository.SaveChangesAsync();

                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.User = _mapper.Map<UserVm>(user);
                return response;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error deleting user: " + ex.Message);
                return response;
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
