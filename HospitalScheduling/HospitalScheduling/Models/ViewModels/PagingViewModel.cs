using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class PagingViewModel
    {
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int NumberPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => (CurrentPage > 1);
        public bool HasNextPage => (CurrentPage < NumberPages);

        public PagingViewModel()
        {
            PageSize = 5;
        }
    }
}
