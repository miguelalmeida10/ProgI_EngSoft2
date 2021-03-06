﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models.ViewModels
{
    public class NurseViewModel
    {
        public IEnumerable<Nurse> Nurses { get; set; }
        public PagingViewModel Pagination { get; set; }
    }
}
