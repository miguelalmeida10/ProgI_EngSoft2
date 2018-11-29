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

                if (db.DoctorsBySpeciality.Any()) return;

                db.DoctorsBySpeciality.AddRange(
                    new Speciality {  Name = "Emergency Medicine" },
                    new Speciality {  Name = "Surgery-General" },
                    new Speciality {  Name = "Pediatrics" },
                    new Speciality {  Name = "Biochemical Genetics" },
                    new Speciality { Name = "Psychiatry" }

                 );
                db.SaveChanges();
            }
        }
    }
}