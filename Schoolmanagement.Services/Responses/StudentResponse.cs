using System.Collections.Generic;
using System.Linq;
using Schoolmanagement.Services.ViewModel.Student;
using Schoolmangenment.Repository.Data;

namespace Schoolmanagement.Services.Responses
{
    public class StudentResponse
    {
        private List<string> _errors = new List<string>();
        public SchoolmanagementRequestStatus RequestStatus { get; set; }

        public IEnumerable<string?> Errors => _errors;
        public bool HasErrors => _errors != null && _errors.Any();

        public void AddError(string message)
        {
            if (_errors == null) _errors = new List<string>();
            _errors.Add(message);
        }

        public void AddError(List<string> messages)
        {
            if (_errors == null) _errors = new List<string>();
            foreach (var item in messages) AddError(item);
        }

        public StudentVm? Student { get; set; }
    }
}
