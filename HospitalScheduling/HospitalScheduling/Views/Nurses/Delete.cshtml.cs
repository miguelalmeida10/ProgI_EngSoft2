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
    public class DeleteModel : DI_BaseViewModel
    {
        private readonly HospitalScheduling.Data.ApplicationDbContext _context;

        public DeleteModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Nurse Nurse { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Nurse = await Context.Nurse.FirstOrDefaultAsync(
                                             m => m.EmployeeID == id);

            if (Nurse == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, Nurse,
                                                     NurseOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Nurse = await Context.Nurse.FindAsync(id);

            var contact = await Context
                .Nurse.AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (contact == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, contact,
                                                     NurseOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Context.Nurse.Remove(Nurse);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
