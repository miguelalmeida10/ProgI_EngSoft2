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
                    new SpecialityforDocs {  Name = "Emergency Medicine" },
                    new SpecialityforDocs {  Name = "Surgery-General" },
                    new SpecialityforDocs {  Name = "Pediatrics" },
                    new SpecialityforDocs {  Name = "Biochemical Genetics" },
                    new SpecialityforDocs { Name = "Psychiatry" }

                 );
                db.SaveChanges();
            }
        }
    }
}