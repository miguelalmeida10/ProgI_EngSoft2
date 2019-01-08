using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models

{
    public class DoctorSpecialities
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorSpecialityID { get; set; }

        [ForeignKey("FK_DoctorID")]
        public int DoctorID { get; set; }
        
        public Doctor Doctor { get; set; }

        [ForeignKey("FK_SpecialityID")]
        public int SpecialityID { get; set; }

        public Speciality Speciality { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }
    }
}
