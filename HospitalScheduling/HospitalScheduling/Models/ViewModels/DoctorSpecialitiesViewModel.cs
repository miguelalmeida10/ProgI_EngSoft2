using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class DoctorSpecialitiesViewModel
    {
        public IEnumerable<DoctorSpecialities> DoctorSpecialities { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
