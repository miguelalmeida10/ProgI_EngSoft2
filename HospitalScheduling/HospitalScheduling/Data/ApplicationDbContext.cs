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
        public DbSet<HospitalScheduling.Models.Doctor> Doctor { get; set; }
        public DbSet<HospitalScheduling.Models.Nurse> Nurse { get; set; }
        public DbSet<HospitalScheduling.Models.HollidayForm> HollidayForm { get; set; }

        public DbSet<HospitalScheduling.Models.SpecialityDocs> SpecialityforDocs  { get; set; }

        public DbSet<HospitalScheduling.Models.RuleModel> RuleModel { get; set; }
        
    }
}
