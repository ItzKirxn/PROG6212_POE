// File:    RequireRoleAttribute.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Action filter that rejects requests whose session Role does not
//          match the required role. Returns 401 if not logged in, 403 if the
//          wrong role.
// Attribution: Action filter pattern — Microsoft Learn, "Filters in ASP.NET Core":
//   https://learn.microsoft.com/aspnet/core/mvc/controllers/filters


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaceDay.Api.Models;

namespace RaceDay.Api.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RequireRoleAttribute : ActionFilterAttribute
{
    private readonly UserRole _role;

    public RequireRoleAttribute(UserRole role) => _role = role;

    public override void OnActionExecuting(ActionExecutingContext ctx)
    {
        if (ctx.HttpContext.GetUserId() == null)
        {
            ctx.Result = new UnauthorizedObjectResult("Not logged in.");
            return;
        }

        if (ctx.HttpContext.GetRole() != _role)
        {
            ctx.Result = new ObjectResult($"This endpoint requires the {_role} role.")
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}