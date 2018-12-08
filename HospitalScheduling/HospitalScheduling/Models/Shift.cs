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
        [Display(Name="Shifts Name")]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ShiftStartHour { get; set; }
        public int DurationMinutes { get; set; }
        public int DurationHours { get; set; }
        public bool Active { get; set; }
        public bool Ended { get; set; }

        [Display(Name = "Doctors of the shift")]
        public IEnumerable<DoctorShifts> Doctors { get; set; }

        [Display(Name = "Previous Doctors of the shift")]
        public IEnumerable<PastShifts> PreviousDoctors { get; set; }
    }
}
