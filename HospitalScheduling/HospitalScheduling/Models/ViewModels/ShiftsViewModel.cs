using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class ShiftsViewModel
    {
        public IEnumerable<Shift> Shifts { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
