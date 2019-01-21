using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class Vacations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VacationID { get; set; }

        [Required(ErrorMessage = "Please enter the starting date of your vacations")]
        [StringLength(10, MinimumLength = 3)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the ending date of your vacations")]
        [StringLength(10, MinimumLength = 3)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public string EndDate { get; set; }

        [ForeignKey("FK_Vacations_DocID")]
        public int DoctorID { get; set; }

        public Doctor Doctor { get; set; }
    }
}
