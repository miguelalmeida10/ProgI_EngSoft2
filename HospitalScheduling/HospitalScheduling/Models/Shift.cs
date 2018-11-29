using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class Shift
    {
        public int ShiftID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationMinutes { get; set; }
        public int DurationSeconds { get; set; }
        public int DurationHours { get; set; }

        [Display(Name = "Doctors of the shift")]
        public IEnumerable<DoctorShifts> Doctors { get; set; }
    }
}
