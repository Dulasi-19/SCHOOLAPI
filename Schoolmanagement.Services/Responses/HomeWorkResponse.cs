using System.Collections.Generic;
using System.Linq;
using Schoolmanagement.Services.ViewModel.HomeWork;
using Schoolmangenment.Repository.Data;

namespace Schoolmanagement.Services.Responses
{
    public class HomeWorkResponse
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

        public HomeWorkVm? HomeWork { get; set; }
    }
}