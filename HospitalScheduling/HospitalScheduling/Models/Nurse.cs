﻿using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalScheduling.Models
{
    public class Nurse : Employee
    {
        [Key]
        public int EmployeeID { get; set; }

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

        //birthday
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Birthday { get; set; }

        //adress
        [Display(Name="Address")]
        public string Adress { get; set; }
    }
}
