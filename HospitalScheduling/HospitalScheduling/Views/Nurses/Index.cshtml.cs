using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models;
using HospitalScheduling.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HospitalScheduling.Views.Nurses
{
    public class IndexModel : DI_BaseViewModel
    {
        private readonly HospitalScheduling.Data.ApplicationDbContext _context;

        public IndexModel(
       ApplicationDbContext context,
       IAuthorizationService authorizationService,
       UserManager<IdentityUser> userManager)
       : base(context, authorizationService, userManager)
        {
        }

        public IList<Nurse> Nurse { get;set; }

        public async Task OnGetAsync()
        {
            var nurses = from c in Context.Nurse
                           select c;

            var isAuthorized = User.IsInRole(Constants.NurseManagersRole) ||
                               User.IsInRole(Constants.NurseAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                nurses = nurses.Where(c => c.Status == EmpStatus.Approved
                                            || c.OwnerID == currentUserId);
            }

            Nurse = await nurses.ToListAsync();
        }
    }
}
