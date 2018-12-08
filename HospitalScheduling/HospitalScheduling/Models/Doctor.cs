using HospitalScheduling.ModelAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalScheduling.Models
{
    public class Doctor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorID { get; set; }


        [RegularExpression(@"\d{7}(\s\d{1})?", ErrorMessage = "Invalid Number")]
        //Numero da Ordem
        public string DoctorNumber { get; set; }

        [RegularExpression(@"([A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ\s]+)", ErrorMessage = "Invalid Name")]
        [Display(Name = "Doctors Name")]
        public string Name { get; set; }

        [RegularExpression(@"(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})", ErrorMessage = "Invalid Email ")]
        public string Email { get; set; }

        [RegularExpression(@"\d{8}(\s\d{1})?", ErrorMessage = "Invalid Citizen Card")]


        public string CC { get; set; }

        //phone
        [Required(ErrorMessage = "Please enter the phone number")]
        [RegularExpression(@"(9[1236]|2\d)\d{7}")]
        public string Phone { get; set; }

        //abc
        [Required(ErrorMessage = "Please enter a Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Birthday { get; set; }

        public int WeeklyHours { get; set; }

        public int WorkingDays { get; set; }

        public bool DoesNightShifts { get; set; }

        public DateTime LastWorkDay { get; set; }

        [Display(Name="Address")]
        public string Address { get; set; }
        
        [ForeignKey("FK_SpecialityID")]
        public int SpecialityID { get; set; }
        public Speciality Speciality { get; set; }

        [Display(Name = "Shifts of the doctor")]
        public IEnumerable<DoctorShifts> Shifts { get; set; }

        [Display(Name = "Past Shifts of the doctor")]
        public IEnumerable<PastShifts> PreviousShifts { get; set; }
    }
}
