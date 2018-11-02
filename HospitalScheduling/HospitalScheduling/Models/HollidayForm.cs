using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class HollidayForm
    {
        [Key]
        public int VacationID { get; set; }

        [Required(ErrorMessage = "Please, enter the starting date of your vacation")]
        [StringLength(50, MinimumLength = 3)]
        public string DateStart { get; set; }

        [Required(ErrorMessage = "Please, enter the ending date of your vacation")]
        public string DateEnd { get; set; }

        
    }
}
