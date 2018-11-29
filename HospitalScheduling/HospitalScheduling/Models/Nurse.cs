using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalScheduling.Models
{
    public class Nurse
    {
        [Key]
        public int NurseID { get; set; }

        [RegularExpression(@"[E]\d+", ErrorMessage = "Invalid Number")]
        //Numero da Ordem
        public string NurseNumber { get; set; }

        //name
        [Required(ErrorMessage = "Please enter the name of the Nurse")]
        [StringLength(50,MinimumLength=3)]
        public string Name { get; set; }

        //email
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress]
        public string Email { get; set; }

        //phone
        [Required(ErrorMessage = "Please enter the phone number")]
        [RegularExpression(@"(9[1236]|2\d)\d{7}")]
        public string Phone { get; set; }

        //abc
        [DataType(DataType.Date, ErrorMessage = "Please enter a Birthday")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Birthday { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Please enter a Birthday")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? BirthdaySon { get; set; }

        //Address
        [Display(Name="Address")]
        public string Address { get; set; }

        [Required]
        public bool? Sons { get; set; }


        [RegularExpression(@"\d{8}(\s\d{1})?", ErrorMessage = "Invalid Citizen Card")]
        public string CC { get; set; }
    }
}
