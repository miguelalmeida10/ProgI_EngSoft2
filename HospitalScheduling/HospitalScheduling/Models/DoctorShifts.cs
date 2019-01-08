using HospitalScheduling.ModelAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class DoctorShifts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorShiftID { get; set; }

        [ForeignKey("FK_DoctorID")]
        public int DoctorID { get; set; }

        [ValidShift]
        public Doctor Doctor { get; set; }

        [ForeignKey("FK_ShiftID")]
        public int ShiftID { get; set; }

        public Shift Shift { get; set; }
    }
}
