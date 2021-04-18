using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.DtoParameters
{
    public class CompanyDtoParameters
    {
        public string CompanyName { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pagesize = 5;

        public int PageSize
        {
            get { return _pagesize; }
            set
            {
                if (value > MaxPageSize)
                    _pagesize = value;
                else
                    _pagesize = value;
            }
        }

        const int MaxPageSize = 20;
    }
}
