using HospitalScheduling.Data;
using HospitalScheduling.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Data
{
    public static class SeedData
    {
        public static void Populate(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();


                if (!db.Speciality.Any())
                {
                    EnsureSpecialitiesPopulated(db);
                }

                if (!db.Nurse.Any())
                {
                    EnsureNursesPopulated(db);
                }

                if (!db.Doctor.Any())
                {
                    EnsureDoctorsPopulated(db);
                }

                if (!db.Shift.Any())
                {
                    EnsureShiftPopulated(db);
                }
            }
        }

        private static void EnsureShiftPopulated(ApplicationDbContext db)
        {
            db.Shift.AddRange(
                 new Shift { Name = "Manha", StartDate = new DateTime(2018, 4, 1, 9, 0, 0), EndDate = new DateTime(9999, 1, 1, 1, 0, 0), ShiftStartHour = new DateTime(0), DurationHours = 5, DurationMinutes = 30, Ended = false },
                 new Shift { Name = "Meio Dia", StartDate = new DateTime(2018, 4, 1, 14, 30, 0), EndDate = new DateTime(9999, 1, 1, 1, 0, 0), ShiftStartHour = new DateTime(0), DurationHours = 5, DurationMinutes = 15, Ended = false },
                 new Shift { Name = "Tarde", StartDate = new DateTime(2018, 4, 1, 19, 45, 50), EndDate = new DateTime(9999, 1, 1, 1, 0, 0), ShiftStartHour = new DateTime(0), DurationHours = 5, DurationMinutes = 30, Ended = false },
                 new Shift { Name = "Noite", StartDate = new DateTime(2018, 4, 1, 19, 45, 50), EndDate = new DateTime(9999, 1, 1, 1, 0, 0), ShiftStartHour = new DateTime(0), DurationHours = 5, DurationMinutes = 30, Ended = false }
             );

            db.SaveChanges();
        }

        private static void EnsureNursesPopulated(ApplicationDbContext db)
        {
            db.Nurse.AddRange(
                new Nurse { CC = "10205101", NurseNumber = "10205101", Name = "Diogo", Email = "diogo@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count()+1)},
                new Nurse { CC = "11018610", NurseNumber = "11018610", Name = "Miguel", Email = "miguel@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10201240", NurseNumber = "10201240", Name = "Maria", Email = "maria@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10212410", NurseNumber = "10212410", Name = "Ana", Email = "ana@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "12312547", NurseNumber = "12312547", Name = "João", Email = "joao@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10212770", NurseNumber = "10212770", Name = "Pedro", Email = "pedro@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10201010", NurseNumber = "10201010", Name = "Inês", Email = "ines@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "41241770", NurseNumber = "41241770", Name = "Noel", Email = "noel@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10114010", NurseNumber = "10114010", Name = "Rita", Email = "rita@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10221410", NurseNumber = "10221410", Name = "Mariana", Email = "mariana@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10513010", NurseNumber = "10513010", Name = "Mario", Email = "mario@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10161010", NurseNumber = "10161010", Name = "Zorlak", Email = "zorlak@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) }

                );


            db.SaveChanges();
        }

        private static void EnsureSpecialitiesPopulated(ApplicationDbContext db)
        {
            db.Speciality.AddRange(
                 new Speciality { Name = "Emergency Medicine" },
                 new Speciality { Name = "Surgery-General" },
                 new Speciality { Name = "Pediatrics" },
                 new Speciality { Name = "Biochemical Genetics" },
                 new Speciality { Name = "Psychiatry" }

              );

            db.SaveChanges();
        }

        private static void EnsureDoctorsPopulated(ApplicationDbContext db)
        {
            db.Doctor.AddRange(
                 new Doctor { CC = "10205101", DoctorNumber = "10205101", Name = "Dr Diogo", Email = "diogo@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "11018610", DoctorNumber = "11018610", Name = "Dr Miguel", Email = "miguel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                new Doctor { CC = "10201240", DoctorNumber = "10201240", Name = "Dr Maria", Email = "maria@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 5 },
                new Doctor { CC = "10212410", DoctorNumber = "10212410", Name = "Dr Ana", Email = "ana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "12312547", DoctorNumber = "12312547", Name = "Dr João", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10212770", DoctorNumber = "10212770", Name = "Dr Pedro", Email = "pedro@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 5 },
                new Doctor { CC = "10201010", DoctorNumber = "10201010", Name = "Dr Inês", Email = "ines@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "41241770", DoctorNumber = "41241770", Name = "Dr Noel", Email = "noel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 4 },
                new Doctor { CC = "10114010", DoctorNumber = "10114010", Name = "Dr Rita", Email = "rita@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                new Doctor { CC = "10221410", DoctorNumber = "10221410", Name = "Dr Mariana", Email = "mariana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10513010", DoctorNumber = "10513010", Name = "Dr Mario", Email = "mario@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "10161010", DoctorNumber = "10161010", Name = "Dr Zorlak", Email = "zorlak@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 4 });


            db.SaveChanges();
        }
    }
}