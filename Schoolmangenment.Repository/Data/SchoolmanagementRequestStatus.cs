using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolmangenment.Repository.Data
{
    public enum SchoolmanagementRequestStatus
    {
        Success = 1,
        Error = 2,
        ValidationError = 3,
        NotFound = 4
    }
}
