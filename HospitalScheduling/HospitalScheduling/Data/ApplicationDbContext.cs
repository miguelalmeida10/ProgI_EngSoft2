using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Models;

namespace HospitalScheduling.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region 1:N
            builder.Entity<Doctor>()
                    .HasOne(d => d.Speciality)
                    .WithMany(s => s.Doctors)
                    .HasForeignKey(d => d.SpecialityID)
                    .HasConstraintName("FK_SpecialityID");
            #endregion

            #region N:M
            builder.Entity<DoctorShifts>()
                    .HasOne(ds => ds.Doctor)
                    .WithMany(d => d.Shifts)
                    .HasForeignKey(ds => ds.DoctorID);

            builder.Entity<DoctorShifts>()
                    .HasOne(ds => ds.Shift)
                    .WithMany(d => d.Doctors)
                    .HasForeignKey(ds => ds.ShiftID);

            builder.Entity<PastShifts>()
                    .HasOne(ds => ds.Doctor)
                    .WithMany(d => (d.PreviousShifts))
                    .HasForeignKey(ds => ds.DoctorID);

            builder.Entity<PastShifts>()
                    .HasOne(ds => ds.Shift)
                    .WithMany(d => (d.PreviousDoctors))
                    .HasForeignKey(ds => ds.ShiftID);
            #endregion

            base.OnModelCreating(builder);
        }

        public DbSet<HospitalScheduling.Models.Doctor> Doctor { get; set; }
        public DbSet<HospitalScheduling.Models.Nurse> Nurse { get; set; }
        public DbSet<HospitalScheduling.Models.HollidayForm> HollidayForm { get; set; }

        public DbSet<HospitalScheduling.Models.Speciality> Speciality  { get; set; }

        public DbSet<HospitalScheduling.Models.RuleModel> RuleModel { get; set; }

        public DbSet<HospitalScheduling.Models.Shift> Shift { get; set; }

        public DbSet<HospitalScheduling.Models.DoctorShifts> DoctorShifts { get; set; }

        public DbSet<HospitalScheduling.Models.PastShifts> PastShifts { get; set; }

    }
}
