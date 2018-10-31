using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HospitalScheduling.Authorization;

namespace HospitalScheduling.Views.Nurses
{
    public class EditModel : DI_BaseViewModel
    {
        private readonly HospitalScheduling.Data.ApplicationDbContext _context;

        public EditModel(
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var nurse = await Context
                .Nurse.AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (nurse == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, nurse,
                                                     NurseOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Nurse.OwnerID = nurse.OwnerID;

            Context.Attach(Nurse).State = EntityState.Modified;

            if (nurse.Status == EmpStatus.Approved)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        nurse,
                                        NurseOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    nurse.Status = EmpStatus.Submitted;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool NurseExists(int id)
        {
            return _context.Nurse.Any(e => e.EmployeeID == id);
        }
    }
}
