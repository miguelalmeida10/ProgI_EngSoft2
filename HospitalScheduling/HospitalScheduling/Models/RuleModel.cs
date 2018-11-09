using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class RuleModel
    {
        //rule name
        [Required(ErrorMessage = "Please enter the rule name")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        //age
        public int age { get; set; }

        //person that we aply the rule
        public bool? category { get; set; }

        //date of the rule
        public DateTime today { get; set; }



    }
}
