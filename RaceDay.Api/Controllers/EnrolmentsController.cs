// File:    EnrolmentsController.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Allows Participants to enter events, and Organisers to view 
//          enrolments for their events.


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Helpers;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrolmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    public EnrolmentsController(AppDbContext db) => _db = db;

    /// <summary>Participant enrols in an event. Participant only.</summary>
    [HttpPost]
    [RequireRole(UserRole.Participant)]
    public async Task<IActionResult> Enrol([FromBody] EnrolDto dto)
    {
        var participantId = HttpContext.GetUserId()!.Value;

        // Check if already enrolled
        if (await _db.Enrolments.AnyAsync(e => e.ParticipantId == participantId && e.EventId == dto.EventId))
            return Conflict("You are already enrolled in this event.");

        // Check if category belongs to the event
        var category = await _db.Categories.FindAsync(dto.CategoryId);
        if (category == null || category.EventId != dto.EventId)
            return BadRequest("Invalid category for this event.");

        var enrolment = new Enrolment
        {
            ParticipantId = participantId,
            EventId = dto.EventId,
            CategoryId = dto.CategoryId
        };

        _db.Enrolments.Add(enrolment);
        await _db.SaveChangesAsync();

        return Ok(enrolment);
    }

    /// <summary>View own enrolments. Participant only.</summary>
    [HttpGet("my")]
    [RequireRole(UserRole.Participant)]
    public async Task<IActionResult> MyEnrolments()
    {
        var pid = HttpContext.GetUserId()!.Value;
        var list = await _db.Enrolments
            .Include(e => e.Event)
            .Include(e => e.Category)
            .Where(e => e.ParticipantId == pid)
            .ToListAsync();
        return Ok(list);
    }

    /// <summary>View all enrolments for your events. Organiser only.</summary>
    [HttpGet("for-my-events")]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> ForMyEvents()
    {
        var orgId = HttpContext.GetUserId()!.Value;
        var list = await _db.Enrolments
            .Include(e => e.Event)
            .Include(e => e.Participant)
            .Include(e => e.Category)
            .Where(e => e.Event.OrganiserId == orgId)
            .ToListAsync();
        return Ok(list);
    }
}