// File:    AuthController.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Registration, login, logout, and "who am I" endpoints. Sessions
//          are used to maintain authenticated state; passwords are hashed
//          before storage.
// Attribution:
//   - Controller conventions: Microsoft Learn, "ASP.NET Core Web API":
//     https://learn.microsoft.com/aspnet/core/web-api/
//   - Session-based auth: Microsoft Learn, "Session and state management":
//     https://learn.microsoft.com/aspnet/core/fundamentals/app-state
//   - Endpoint design from the author's own Part 1 endpoint plan.


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Helpers;
using RaceDay.Api.Models;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasherService _hasher;

    public AuthController(AppDbContext db, IPasswordHasherService hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    /// <summary>Register a new Organiser or Participant.</summary>
    /// <response code="200">User created successfully.</response>
    /// <response code="400">Missing required fields.</response>
    /// <response code="409">Email already registered.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Email and password are required.");

        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("Email already registered.");

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = _hasher.Hash(dto.Password),
            Role = dto.Role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { user.Id, user.Email, user.Role });
    }

    /// <summary>Log in with email and password; starts a session.</summary>
    /// <response code="200">Session created.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Role", user.Role.ToString());

        return Ok(new { user.Id, user.Email, user.Role });
    }

    /// <summary>Log out — clears the session.</summary>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    /// <summary>Get the currently logged-in user.</summary>
    /// <response code="200">User details.</response>
    /// <response code="401">Not logged in.</response>
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = HttpContext.GetUserId();
        if (id == null) return Unauthorized();

        var user = await _db.Users.FindAsync(id);
        if (user == null) return Unauthorized();

        return Ok(new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role
        });
    }
}