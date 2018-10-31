using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HospitalScheduling.Data;
using HospitalScheduling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HospitalScheduling.Authorization;

namespace HospitalScheduling.Views.Nurses
{
    public class DetailsModel : DI_BaseViewModel
    {
        private readonly HospitalScheduling.Data.ApplicationDbContext _context;

        public DetailsModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public Nurse Nurse { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Nurse = await Context.Nurse.FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (Nurse == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.NurseManagersRole) ||
                               User.IsInRole(Constants.NurseAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Nurse.OwnerID
                && Nurse.Status != EmpStatus.Approved)
            {
                return new ChallengeResult();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id, EmpStatus status)
        {
            var contact = await Context.Nurse.FirstOrDefaultAsync(
                                                      m => m.EmployeeID == id);

            if (contact == null)
            {
                return NotFound();
            }

            var contactOperation = (status == EmpStatus.Approved)
                                                       ? NurseOperations.Approve
                                                       : NurseOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact,
                                        contactOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            contact.Status = status;
            Context.Nurse.Update(contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
