using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class SpecialitiesViewModel
    {
        public IEnumerable<Speciality> Specialities { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
