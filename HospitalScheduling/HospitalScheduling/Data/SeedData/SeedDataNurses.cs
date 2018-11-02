using HospitalScheduling.Data;
using HospitalScheduling.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Data
{
    public static class SeedDataNurses
    {
        public static void Populate(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (db.Nurse.Any()) return;

                db.Nurse.AddRange(
                    new Nurse { Name = "Diogo", Email = "diogo@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Miguel", Email = "miguel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Maria", Email = "maria@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Ana", Email = "ana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "João", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Pedro", Email = "pedro@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Inês", Email = "ines@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Noel", Email = "noel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Rita", Email = "rita@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Mariana", Email = "mariana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Mario", Email = "mario@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" },
                    new Nurse { Name = "Zorlak", Email = "zorlak@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Adress = "Rua do Volta a tras" }

                    );


                db.SaveChanges();
            }
        }
    }
}