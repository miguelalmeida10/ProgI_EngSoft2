using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models
{
    public class RulesSchedule
    {
        public int Id { get; set; }

        //name
        [Required(ErrorMessage = "Please enter the name of the Employ")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }




    }
}
