using HospitalScheduling.Data;
using HospitalScheduling.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Data
{
    public static class SeedDataDoctors
    {
        public static void Populate(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (db.Doctor.Any()) return;

                db.Doctor.AddRange(
                     new Doctor { CC = "10205101", DoctorNumber = "10205101", Name = "Dr Diogo", Email = "diogo@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" , SpecialityID = 1},
                    new Doctor { CC = "11018610", DoctorNumber = "11018610", Name = "Dr Miguel", Email = "miguel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                    new Doctor { CC = "10201240", DoctorNumber = "10201240", Name = "Dr Maria", Email = "maria@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 0 },
                    new Doctor { CC = "10212410", DoctorNumber = "10212410", Name = "Dr Ana", Email = "ana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                    new Doctor { CC = "12312547", DoctorNumber = "12312547", Name = "Dr João", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                    new Doctor { CC = "10212770", DoctorNumber = "10212770", Name = "Dr Pedro", Email = "pedro@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 0 },
                    new Doctor { CC = "10201010", DoctorNumber = "10201010", Name = "Dr Inês", Email = "ines@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                    new Doctor { CC = "41241770", DoctorNumber = "41241770", Name = "Dr Noel", Email = "noel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 4 },
                    new Doctor { CC = "10114010", DoctorNumber = "10114010", Name = "Dr Rita", Email = "rita@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                    new Doctor { CC = "10221410", DoctorNumber = "10221410", Name = "Dr Mariana", Email = "mariana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                    new Doctor { CC = "10513010", DoctorNumber = "10513010", Name = "Dr Mario", Email = "mario@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1},
                    new Doctor { CC = "10161010", DoctorNumber = "10161010", Name = "Dr Zorlak", Email = "zorlak@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 4});


                db.SaveChanges();
            }
        }
    }
}