using HospitalScheduling.Data;
using HospitalScheduling.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Data
{
    public static class SeedData
    {
        public static void PopulateAsync(IServiceProvider applicationServices)
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
                    AddVacationDays(db);
                }

                if (!db.Vacations.Any())
                {
                    EnsureVacationsPopulated(db);
                }

                EnsureShiftPopulated(db);
            }
        }

        private static void EnsureVacationsPopulated(ApplicationDbContext db)
        {
            // Adding 12 vacations for 12 doctors starting on random dates and ending on random dates
            db.Vacations.AddRange(
                new Vacations { DoctorID = 1, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 2, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 3, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 4, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 5, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 6, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 7, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 8, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 9, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 10, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 11, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) },
                new Vacations { DoctorID = 12, StartDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20), EndDate = new Random().Next(1, 31) + "/" + new Random().Next(1, 12) + "/" + new Random().Next(19, 20) }
            );

            db.SaveChanges();
        }

        private static void AddVacationDays(ApplicationDbContext db)
        {
            List<Doctor> ListOfDoctorsToBeUpdated = new List<Doctor>();
            foreach (Doctor doctor in db.Doctor)
            {
                var age = DateTime.Now.Year - doctor.Birthday.Year;

                // Until doctors reach 39 years old, they get 25 days of vacations
                if (age<39)
                {
                    doctor.VacationDays = 25;
                }

                // Until doctors reach 49 years old, they get 26 days of vacations
                if (age >= 39 && age < 49)
                {
                    doctor.VacationDays = 26;
                }

                // Until doctors reach 59 years old, they get 27 days of vacations
                if (age >= 49 && age < 59)
                {
                    doctor.VacationDays = 27;
                }

                // Doctors older than 59 years old, get 28 days of vacations
                if (age >= 59)
                {
                    doctor.VacationDays = 28;
                }
            }
            db.Doctor.UpdateRange(ListOfDoctorsToBeUpdated);
            db.SaveChanges();
        }

        public static async Task EnsureRolesPopulatedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Administrator", "Nurse", "Doctor" };

            foreach (var nome in roles)
            {
                var roleExists = await RoleManager.RoleExistsAsync(nome);
                if (!roleExists)
                {
                    await RoleManager.CreateAsync(new IdentityRole(nome));
                }
            }
        }

        private static void EnsureShiftPopulated(ApplicationDbContext db)
        {
            Task newShifts = Task.Run(() =>
            {
                if (db.Shift.Count() == 0 || db.Shift.LastOrDefault().StartDate.Year < DateTime.Now.Year)
                {
                    List<Shift> shiftsOfYear = new List<Shift>();
                    for (int i = DateTime.Now.Month; i <= 12; i++)
                    {
                        int maxDias = (DateTime.IsLeapYear(DateTime.Now.Year) && i == 2) ? 29 : 28;
                        int maxDiasMes = 0;

                        switch (i)
                        {
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                maxDiasMes = 30;
                                break;
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                maxDiasMes = 31;
                                break;
                        };

                        maxDias = (!DateTime.IsLeapYear(DateTime.Now.Year) && i == 2) ? 28 : maxDiasMes;

                        for (int j = DateTime.Now.Day; j <= maxDias; j++)
                        {
                            shiftsOfYear.Add(new Shift { Name = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0).ToLongDateString() + " - Manha", StartDate = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0), Active = (j == DateTime.Now.Day ? true : false), Ended = false });
                            shiftsOfYear.Add(new Shift { Name = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0).ToLongDateString() + " - Tarde", StartDate = new DateTime(DateTime.Now.Year, i, j, 16, 0, 0), Active = (j == DateTime.Now.Day ? true : false), Ended = false });
                            shiftsOfYear.Add(new Shift { Name = new DateTime(DateTime.Now.Year, i, j, 8, 0, 0).ToLongDateString() + " - Noite", StartDate = new DateTime(DateTime.Now.Year, i, j, 0, 0, 0), Active = (j == DateTime.Now.Day ? true : false), Ended = false });
                        }
                    }
                    db.Shift.AddRange(shiftsOfYear);
                }
            });

            newShifts.Wait();

            db.SaveChanges();
        }

        private static void EnsureNursesPopulated(ApplicationDbContext db)
        {
            db.Nurse.AddRange(
                new Nurse { CC = "10205101", NurseNumber = "10205101", Name = "Diogo Miguel Afonso Sa", Email = "diogo@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "11018610", NurseNumber = "11018610", Name = "Miguel Diogo João", Email = "miguel@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10201240", NurseNumber = "10201240", Name = "Maria Afonso Castro", Email = "maria@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1964"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10212410", NurseNumber = "10212410", Name = "Ana Inês Silva Castro", Email = "ana@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "12312547", NurseNumber = "12312547", Name = "João Pedro Dinis", Email = "joao@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10212770", NurseNumber = "10212770", Name = "Pedro Noel Santos Lopes", Email = "pedro@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10201010", NurseNumber = "10201010", Name = "Inês Mariana Da Silva", Email = "ines@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "41241770", NurseNumber = "41241770", Name = "Noel Mario Paulo Nunes", Email = "noel@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10114010", NurseNumber = "10114010", Name = "Rita da Silva Sa", Email = "rita@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10221410", NurseNumber = "10221410", Name = "Mariana Castanheira Gabriel Lopes", Email = "mariana@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "10513010", NurseNumber = "10513010", Name = "Mario Simoes Antunes Silva", Email = "mario@email.com", Phone = "912345678", Sons = true, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) },
                new Nurse { CC = "12312547", NurseNumber = "12312547", Name = "João Pedro Rodrigues", Email = "joao@email.com", Phone = "912345678", Sons = false, Birthday = DateTime.Parse("11/11/1995"), BirthdaySon = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = new Random().Next(1, db.Speciality.Count() + 1) }
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
                new Doctor { CC = "10205101", DoctorNumber = "10205101", Name = "Diogo Miguel Dinis", Email = "diogo@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "11018610", DoctorNumber = "11018610", Name = "Miguel Diogo João", Email = "miguel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                new Doctor { CC = "10201240", DoctorNumber = "10201240", Name = "Maria Afonso Castro", Email = "maria@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 5 },
                new Doctor { CC = "10212410", DoctorNumber = "10212410", Name = "Ana Inês Silva Castro", Email = "ana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "12312547", DoctorNumber = "12312547", Name = "João Pedro Dinis", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10212770", DoctorNumber = "10212770", Name = "Pedro Noel Santos Lopes", Email = "pedro@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 5 },
                new Doctor { CC = "10201010", DoctorNumber = "10201010", Name = "Inês Mariana Da Silva", Email = "ines@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "41241770", DoctorNumber = "41241770", Name = "Paulo Nunes Noel Mario", Email = "noel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 4 },
                new Doctor { CC = "10114010", DoctorNumber = "10114010", Name = "Rita da Silva Sa", Email = "rita@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                new Doctor { CC = "10221410", DoctorNumber = "10221410", Name = "Fabio Miguel Teixeira Santos", Email = "mariana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221411", DoctorNumber = "10221411", Name = "Mario Simoes Antunes Silva", Email = "mariano@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221412", DoctorNumber = "10221412", Name = "João Pedro Rodrigues", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221413", DoctorNumber = "10221413", Name = "Mariana Castanheira Lopes Gabriel", Email = "manuel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221414", DoctorNumber = "10221414", Name = "Manuela Cardoso Pinto Simoes", Email = "manuela@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10513010", DoctorNumber = "10513010", Name = "Luciano Gigi Brido", Email = "mario@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "10205101", DoctorNumber = "10205101", Name = "Diogo Miguel Afonso Sa", Email = "diogo@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "11018610", DoctorNumber = "11018610", Name = "Miguel Diogo Noel", Email = "miguel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                new Doctor { CC = "10201240", DoctorNumber = "10201240", Name = "Maria Diogo Castro", Email = "maria@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 5 },
                new Doctor { CC = "10212410", DoctorNumber = "10212410", Name = "Ana Brido Silva Castro", Email = "ana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "12312547", DoctorNumber = "12312547", Name = "João Pedro Dinis Rodrigues", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10212770", DoctorNumber = "10212770", Name = "Cristiano Santos Lopes", Email = "pedro@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 5 },
                new Doctor { CC = "10201010", DoctorNumber = "10201010", Name = "Inês Pedro Da Silva", Email = "ines@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 },
                new Doctor { CC = "41241770", DoctorNumber = "41241770", Name = "Noel Mario Paulo Nunes", Email = "noel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 4 },
                new Doctor { CC = "10114010", DoctorNumber = "10114010", Name = "Rita Maria Sa", Email = "rita@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 2 },
                new Doctor { CC = "10221410", DoctorNumber = "10221410", Name = "Fabio Da Silva Santos", Email = "mariana@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221411", DoctorNumber = "10221411", Name = "Mario Rodrigues Noel Silva", Email = "mariano@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221412", DoctorNumber = "10221412", Name = "João Pedro Nunes Rodrigues", Email = "joao@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221413", DoctorNumber = "10221413", Name = "Mariana Castanheira Gabriel Lopes", Email = "manuel@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10221414", DoctorNumber = "10221414", Name = "Manuela Brido Pinto Simoes", Email = "manuela@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 3 },
                new Doctor { CC = "10513010", DoctorNumber = "10513010", Name = "Gabriel Gigi Inês", Email = "mario@email.com", Phone = "912345678", Birthday = DateTime.Parse("11/11/1995"), Address = "Rua do Volta a tras", SpecialityID = 1 });


            db.SaveChanges();
        }
    }
}