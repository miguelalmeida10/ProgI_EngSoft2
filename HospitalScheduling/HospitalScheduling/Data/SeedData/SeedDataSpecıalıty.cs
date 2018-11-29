using HospitalScheduling.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Data
{
    public static class SeedDataSpecıalıty
    {
        public static void Populate(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (db.SpecialityforDocs.Any()) return;

                db.SpecialityforDocs.AddRange(
                    new SpecialityDocs {  Name = "Emergency Medicine" },
                    new SpecialityDocs {  Name = "Surgery-General" },
                    new SpecialityDocs {  Name = "Pediatrics" },
                    new SpecialityDocs {  Name = "Biochemical Genetics" },
                    new SpecialityDocs { Name = "Psychiatry" }

                 );
                db.SaveChanges();
            }
        }
    }
}