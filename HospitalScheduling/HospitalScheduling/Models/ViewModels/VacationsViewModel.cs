using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class VacationsViewModel
    {
        public IEnumerable<Vacations> Vacations { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
