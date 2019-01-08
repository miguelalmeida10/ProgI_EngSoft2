using HospitalScheduling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.ModelAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class DateGreaterEqualThan : ValidationAttribute
    {
        public string Date { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime = (value == null) ? new DateTime() : (DateTime)value;

            var invalid = false;


            if (!Date.Contains(' ')) { 
                invalid = (Date == null)
                    ? DateTime.Now.Subtract(dateTime).TotalHours < 0
                    : (DateTime.Parse(Date)).Subtract(dateTime).TotalHours < 0;
            }
            else
            {
                int number = int.Parse(Date.Split(' ')[0]);
                string type = Date.Split(' ')[1];
                switch (type.ToLower())
                {
                    case "days":
                        invalid = new DateTime(DateTime.Now.Subtract(new DateTime().AddDays(number)).Ticks).Subtract(dateTime).TotalHours < 0;
                        break;
                    case "months":
                        invalid = new DateTime(DateTime.Now.Subtract(new DateTime().AddMonths(number)).Ticks).Subtract(dateTime).TotalHours < 0;
                        break;
                    case "years":
                        invalid = new DateTime(DateTime.Now.Subtract(new DateTime().AddYears(number)).Ticks).Subtract(dateTime).TotalHours < 0;
                        break;
                }
            }


            if (invalid)
            {
                return new ValidationResult(ErrorMessageString);
            }

            return ValidationResult.Success;
        }
    }
}