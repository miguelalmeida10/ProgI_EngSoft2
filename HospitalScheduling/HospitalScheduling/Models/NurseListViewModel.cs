using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class NurseListViewModel
    {
        public IEnumerable<Nurse> Nurse { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
