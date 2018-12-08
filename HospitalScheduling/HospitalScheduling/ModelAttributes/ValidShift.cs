using HospitalScheduling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidShift : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DoctorShifts doctorshift = (DoctorShifts)validationContext.ObjectInstance;
            /*case "Manha":
            case "Morning":
                break;
            case "Tarde":
            case "Afternoon":
                break;
            case "Noite":
            case "Night":
                break;*/

            

            return ValidationResult.Success;
        }
    }
}
