using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class HollidayFormViewModel
    {
        public IEnumerable<HollidayForm> HollidayForms { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
