using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models

{
    public class Speciality
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecialityID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the speciality name")]
        public string Name { get; set; }

        public IEnumerable<Doctor> Doctors { get; set; }

        [Display(Name = "Previous Doctors of the Speciality")]
        public IEnumerable<DoctorSpecialities> PreviousDoctors { get; set; }
    }
}
