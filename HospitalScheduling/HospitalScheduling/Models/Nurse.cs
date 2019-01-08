using HospitalScheduling.ModelAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalScheduling.Models
{
    public class Nurse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NurseID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an Orders Number")]
        [RegularExpression(@"[E]\d+", ErrorMessage = "Invalid Number")]
        //Numero da Ordem
        public string NurseNumber { get; set; }

        //name
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a name")]
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

        //birthday
        [DateGreaterEqualThan(Date = "18 years", ErrorMessage = "Can't be younger than 18 years")]
        [DateLesserEqualThan(Date = "65 years", ErrorMessage = "Can't be older than 65 years")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a birthday")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Birthday { get; set; }

        [DateLesserEqualThan(ErrorMessage = "The date cant be greater than todays date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter the youngest childs birthday")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? BirthdaySon { get; set; }

        //Address
        [Display(Name="Address")]
        public string Address { get; set; }

        [ForeignKey("FK_SpecialityID")]
        public int SpecialityID { get; set; }
        public Speciality Speciality { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify if the nurse has any children")]
        public bool? Sons { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill in the Citizen Card number")]
        [RegularExpression(@"\d{8}(\s\d{1})?", ErrorMessage = "Invalid Citizen Card")]
        public string CC { get; set; }
    }
}
