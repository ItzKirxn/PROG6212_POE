// File:    ResultsController.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Allows Organisers to capture results, and Participants to view
//          their own results.


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Helpers;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ResultsController(AppDbContext db) => _db = db;

    /// <summary>Capture a result. Organiser (owner) only.</summary>
    [HttpPost]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> Create([FromBody] ResultDto dto)
    {
        var enrolment = await _db.Enrolments
            .Include(e => e.Event)
            .FirstOrDefaultAsync(e => e.Id == dto.EnrolmentId);

        if (enrolment == null) return NotFound("Enrolment not found.");
        if (enrolment.Event.OrganiserId != HttpContext.GetUserId()) return Forbid();

        var result = new Result
        {
            EnrolmentId = dto.EnrolmentId,
            FinishTime = dto.FinishTime,
            Position = dto.Position
        };

        _db.Results.Add(result);
        await _db.SaveChangesAsync();

        return Ok(result);
    }

    /// <summary>View own results. Participant only.</summary>
    [HttpGet("my")]
    [RequireRole(UserRole.Participant)]
    public async Task<IActionResult> MyResults()
    {
        var pid = HttpContext.GetUserId()!.Value;
        var list = await _db.Results
            .Include(r => r.Enrolment).ThenInclude(e => e.Event)
            .Include(r => r.Enrolment).ThenInclude(e => e.Category)
            .Where(r => r.Enrolment.ParticipantId == pid)
            .ToListAsync();
        return Ok(list);
    }

    /// <summary>View all results for your events. Organiser only.</summary>
    [HttpGet("for-my-events")]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> ForMyEvents()
    {
        var orgId = HttpContext.GetUserId()!.Value;
        var list = await _db.Results
            .Include(r => r.Enrolment).ThenInclude(e => e.Event)
            .Include(r => r.Enrolment).ThenInclude(e => e.Participant)
            .Where(r => r.Enrolment.Event.OrganiserId == orgId)
            .ToListAsync();
        return Ok(list);
    }
}