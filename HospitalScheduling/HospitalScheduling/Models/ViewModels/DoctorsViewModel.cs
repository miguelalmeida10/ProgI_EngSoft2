using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class DoctorsViewModel
    {
        public IEnumerable<Doctor> Doctors { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
