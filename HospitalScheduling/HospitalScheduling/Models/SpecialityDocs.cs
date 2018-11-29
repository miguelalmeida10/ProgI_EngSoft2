using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Models

{
    public class SpecialityDocs
    {
        [Key]
        public int SpecialityforDocsID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
