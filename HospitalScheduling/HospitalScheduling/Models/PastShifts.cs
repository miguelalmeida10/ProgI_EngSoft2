using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalScheduling.Models
{
    public class PastShifts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryID { get; set; }

        [ForeignKey("FK_DoctorID")]
        public int DoctorID { get; set; }

        public Doctor Doctor { get; set; }

        [ForeignKey("FK_ShiftID")]
        public int ShiftID { get; set; }

        public Shift Shift { get; set; }

        public DateTime ShiftEndDate { get; set; }

        public PastShifts() { }

        public PastShifts(DoctorShifts doctor) 
        {
            Doctor = doctor.Doctor;
            DoctorID = doctor.DoctorID;
            Shift = doctor.Shift;
            ShiftID = doctor.ShiftID;
            ShiftEndDate = doctor.Shift.StartDate.AddHours(6);
        }
    }
}
