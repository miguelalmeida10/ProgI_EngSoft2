using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class DoctorShiftsViewModel
    {
        public IEnumerable<DoctorShifts> DoctorShifts { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
