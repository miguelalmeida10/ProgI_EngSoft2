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
                    new Nurse { CC = "10205101", NurseNumber = "10205101", Name = "Diogo", Email = "diogo@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "11018610", NurseNumber = "11018610", Name = "Miguel", Email = "miguel@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10201240", NurseNumber = "10201240", Name = "Maria", Email = "maria@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10212410", NurseNumber = "10212410", Name = "Ana", Email = "ana@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "12312547", NurseNumber = "12312547", Name = "João", Email = "joao@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10212770", NurseNumber = "10212770", Name = "Pedro", Email = "pedro@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10201010", NurseNumber = "10201010", Name = "Inês", Email = "ines@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "41241770", NurseNumber = "41241770", Name = "Noel", Email = "noel@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10114010", NurseNumber = "10114010", Name = "Rita", Email = "rita@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10221410", NurseNumber = "10221410", Name = "Mariana", Email = "mariana@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10513010", NurseNumber = "10513010", Name = "Mario", Email = "mario@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" },
                    new Nurse { CC = "10161010", NurseNumber = "10161010", Name = "Zorlak", Email = "zorlak@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras" }

                    );


                db.SaveChanges();
            }
        }
    }
}