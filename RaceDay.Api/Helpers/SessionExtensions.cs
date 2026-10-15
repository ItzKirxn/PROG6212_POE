// File:    SessionExtensions.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Extension methods for reading the authenticated user's id and role
//          out of the current HTTP session.
// Attribution: Session access patterns — Microsoft Learn, "Session and state management":
//   https://learn.microsoft.com/aspnet/core/fundamentals/app-state


using RaceDay.Api.Models;

namespace RaceDay.Api.Helpers;

public static class SessionExtensions
{
    public static int? GetUserId(this HttpContext ctx)
        => ctx.Session.GetInt32("UserId");

    public static UserRole? GetRole(this HttpContext ctx)
    {
        var r = ctx.Session.GetString("Role");
        return r != null && Enum.TryParse<UserRole>(r, out var role)
            ? role
            : null;
    }

    public static bool IsOrganiser(this HttpContext ctx)
        => ctx.GetRole() == UserRole.Organiser;

    public static bool IsParticipant(this HttpContext ctx)
        => ctx.GetRole() == UserRole.Participant;
}