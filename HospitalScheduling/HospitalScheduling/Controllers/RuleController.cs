using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HospitalScheduling.Controllers
{
    public class RuleController : Controller
    {
        public IActionResult age()
        {
            int ageDoc;
            int days;

            if(ageDoc < 39)
            {
                days = 25; 
            } else if (age < 49)
            {
                days = 26;
            } else if (age < 59)
            {
                days = 27;
            } else
            {
                days = 28;
            }

        }
    }
}