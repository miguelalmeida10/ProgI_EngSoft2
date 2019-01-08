using HospitalScheduling.ModelAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{ 
    public class Shift
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the shifts name")]
        [Display(Name="Shifts Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the shifts start date")]
        public DateTime StartDate { get; set; }

        public bool Active { get; set; }

        public bool Ended { get; set; }

        [Display(Name = "Doctors of the shift")]
        public IEnumerable<DoctorShifts> Doctors { get; set; }

        [Display(Name = "Previous Doctors of the shift")]
        public IEnumerable<PastShifts> PreviousDoctors { get; set; }
    }
}
