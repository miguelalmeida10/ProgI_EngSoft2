using HospitalScheduling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidCC : ValidationAttribute
    {
        private Dictionary<char, int> valueAlphabet = new Dictionary<char, int>();

        public ValidCC()
        {
            int i = 10;
            for(char a = 'A'; a < 'Z'; a++)
            {
                valueAlphabet.Add(a, i++);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string CC = value != null ? value as string : "";
            int sum = 0;
            bool nonPairDigit = false;
            try
            {
                if (CC.Length != 12)
                    return new ValidationResult("Invalid size for the citzens card number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ValidationResult("Invalid size for the citzens card number.");
            }

            CC.ToList().ForEach((letter) =>
            {
                int newvalue = (char.IsLetter(letter)? valueAlphabet[letter]:int.Parse(letter.ToString()));
                if (nonPairDigit)
                {
                    newvalue -= ((newvalue*=2)> 9) ? 9 : 0;
                }
                sum += newvalue;
                nonPairDigit = !nonPairDigit;
            });

            if (sum % 10 != 0)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
