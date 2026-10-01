using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schoolmanagement.Services.Responses;
using Schoolmanagement.Services.ViewModel.Student;

namespace Schoolmanagement.Services.Interface
{
    public interface IStudentService : IDisposable
    {
        void ClearChangeTracking();
        Task<IEnumerable<StudentVm>> GetAllStudentsAsync();
        Task<StudentVm?> GetStudentByIdAsync(int id, CancellationToken ct = default);
        Task<StudentResponse> CreateStudentAsync(StudentRequest request, CancellationToken ct = default);
        Task<StudentResponse> UpdateStudentAsync(int id, StudentRequest request, CancellationToken ct = default);
        Task<StudentResponse> DeleteStudentAsync(int id, CancellationToken ct = default);
    }
}
