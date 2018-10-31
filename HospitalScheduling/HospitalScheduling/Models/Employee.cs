using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;


namespace HospitalScheduling.Models
{
    public interface Employee
    {
        int EmployeeID { get; set; }

        string OwnerID { get; set; }


        //[Required(ErrorMessage = "Please enter the name of the worker")]
        //[RegularExpression(@"[A-Z][a-z]+(\s[A-Z][a-z]+)?")]
        string Name { get; set; }

       

        //[Required(ErrorMessage = "Please enter your email")]
        //[EmailAddress]
        string Email { get; set; }


        //[Required(ErrorMessage = "Please enter the phone number")]
        //[RegularExpression(@"(9[1236]|2\d)\d{7}")]
        string Phone { get; set; }

        //data de nascimento 
        //[Required(ErrorMessage = "Please enter your email")]
        DateTime Birthday { get; set; }
        //morada
        string Adress { get; set; }


        
    }
}
