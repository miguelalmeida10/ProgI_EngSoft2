using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalScheduling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HospitalScheduling.Authorization
{
    public class NurseAdministratorsAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, Nurse>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                     Nurse resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.NurseAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
