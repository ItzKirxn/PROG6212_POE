// File:    ProfileController.cs
// Author:  Kieran Pillay — ST10445189
// Date:    17 October 2026
// Purpose: Allows authenticated users to view and update their own profile.


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Helpers;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProfileController(AppDbContext db) => _db = db;

    /// <summary>View own profile.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var id = HttpContext.GetUserId();
        if (id == null) return Unauthorized();

        var user = await _db.Users.FindAsync(id);
        if (user == null) return Unauthorized();

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email, user.Role });
    }

    /// <summary>Update own profile (first name and last name).</summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileDto dto)
    {
        var id = HttpContext.GetUserId();
        if (id == null) return Unauthorized();

        var user = await _db.Users.FindAsync(id);
        if (user == null) return Unauthorized();

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        await _db.SaveChangesAsync();

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email, user.Role });
    }
}