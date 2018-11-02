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
                    new Doctor { Name = "Dr Diogo", Email = "diogo@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Miguel", Email = "miguel@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Maria", Email = "maria@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "DrAna", Email = "ana@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = " Dr João", Email = "joao@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Pedro", Email = "pedro@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Inês", Email = "ines@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Noel", Email = "noel@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Rita", Email = "rita@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Mariana", Email = "mariana@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Mario", Email = "mario@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Doctor { Name = "Dr Zorlak", Email = "zorlak@doctoremail.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" }

                    );


                db.SaveChanges();
            }
        }
    }
}