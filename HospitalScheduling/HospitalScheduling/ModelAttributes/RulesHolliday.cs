using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.ModelAttributes
{
    public class RulesHolliday : ValidationAttribute
    {
        /*
        TimeSpan span;

        public TimeSpanRange()
        {
        span= new TimeSpan(DateTime.Now().Add(22,0,0));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (span.Subtract((TimeSpan)value).Ticks>0)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
    */
    }
}
