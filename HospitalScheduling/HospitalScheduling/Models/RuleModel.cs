using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class RuleModel
    {

        [Key]
        public int ValidationID { get; set; }

        //employ name
        [Required(ErrorMessage = "Please enter the Employ name")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        //Categori
        [Required]
        public string category { get; set; }

        //age
        [Required]
        public int age { get; set; }

        //date of the begin schedule
        public DateTime begin { get; set; }

        //date of the end of schedule 
        public DateTime end { get; set; }

    }
}
