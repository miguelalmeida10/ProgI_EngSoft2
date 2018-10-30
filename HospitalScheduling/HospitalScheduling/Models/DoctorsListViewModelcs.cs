﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class DoctorsListViewModel
    {
        public IEnumerable<Doctor> Doctor { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}