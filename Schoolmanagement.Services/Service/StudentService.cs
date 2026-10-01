using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Student;
using Schoolmangenment.Entities.Model;
using Schoolmangenment.Repository.Data;
using Schoolmangenment.Repository.Interfaces;

namespace Schoolmanagement.Services.Service
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _repository;
        private readonly IMapper _mapper;

        public StudentService(IMapper mapper, IGenericRepository<Student> repository)
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

        public async Task<IEnumerable<StudentVm>> GetAllStudentsAsync()
        {
            var students = await _repository.GetQueryable()
                .Where(s => !s.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<StudentVm>>(students);
        }

        public async Task<StudentVm?> GetStudentByIdAsync(int id, CancellationToken ct = default)
        {
            var student = await _repository.GetQueryable()
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);
            return _mapper.Map<StudentVm>(student);
        }

        public async Task<StudentResponse> CreateStudentAsync(StudentRequest request, CancellationToken ct = default)
        {
            var response = new StudentResponse();
            using var transaction = await _repository.BeginTransactionAsync(ct);
            try
            {
                var student = new Student
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    RollNumber = request.RollNumber,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Address = request.Address,
                    Class = request.Class,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy,
                    IsActive = request.IsActive,
                    IsDeleted = false
                };

                await _repository.AddAsync(student);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync(ct);
                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.Student = _mapper.Map<StudentVm>(student);
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error creating student: " + ex.Message);
                return response;
            }
        }

        public async Task<StudentResponse> UpdateStudentAsync(int id, StudentRequest request, CancellationToken ct = default)
        {
            var response = new StudentResponse();
            try
            {
                var existing = await _repository.GetQueryable()
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);

                if (existing == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Student with ID {id} not found.");
                    return response;
                }

                existing.FirstName = request.FirstName;
                existing.LastName = request.LastName;
                existing.RollNumber = request.RollNumber;
                existing.Email = request.Email;
                existing.PhoneNumber = request.PhoneNumber;
                existing.DateOfBirth = request.DateOfBirth;
                existing.Gender = request.Gender;
                existing.Address = request.Address;
                existing.Class = request.Class;
                existing.UserId = request.UserId;
                existing.IsActive = request.IsActive;
                existing.ModifiedBy = request.ModifiedBy;
                existing.ModifiedDate = DateTime.UtcNow;

                _repository.Update(existing);
                await _repository.SaveChangesAsync();

                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.Student = _mapper.Map<StudentVm>(existing);
                return response;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error updating student: " + ex.Message);
                return response;
            }
        }

        public async Task<StudentResponse> DeleteStudentAsync(int id, CancellationToken ct = default)
        {
            var response = new StudentResponse();
            try
            {
                var student = await _repository.GetQueryable()
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);

                if (student == null)
                {
                    response.RequestStatus = SchoolmanagementRequestStatus.NotFound;
                    response.AddError($"Student with ID {id} not found.");
                    return response;
                }

                student.IsDeleted = true;
                student.IsActive = false;
                student.ModifiedDate = DateTime.UtcNow;

                _repository.Update(student);
                await _repository.SaveChangesAsync();

                response.RequestStatus = SchoolmanagementRequestStatus.Success;
                response.Student = _mapper.Map<StudentVm>(student);
                return response;
            }
            catch (Exception ex)
            {
                response.RequestStatus = SchoolmanagementRequestStatus.Error;
                response.AddError("Error deleting student: " + ex.Message);
                return response;
            }
        }
    }
}
