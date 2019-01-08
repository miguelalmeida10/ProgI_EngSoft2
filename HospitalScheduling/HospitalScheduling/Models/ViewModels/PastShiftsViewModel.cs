using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class PastShiftsViewModel
    {
        public IEnumerable<PastShifts> PastShifts { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
