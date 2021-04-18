using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.DtoParameters
{
    public class EmployeeDtoParameters
    {
        const int MaxPageSize = 20;
        public string Gender { get; set; }
        public string Q { get; set; }
        public int PageNumber { get; set; }
        private int _pageSize = 5;

        public int PageSize
        {
            get { return _pageSize = 5; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public string OrderBy { get; set; } = "Name";
    }
}
