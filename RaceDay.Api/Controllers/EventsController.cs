// File:    EventsController.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Handles event management. Both roles can view; only Organisers
//          can create, update, or delete events (and only their own).


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Helpers;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db) => _db = db;

    /// <summary>List all events. Both roles.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (HttpContext.GetUserId() == null) return Unauthorized();

        var events = await _db.Events
            .Include(e => e.Categories)
            .Include(e => e.Organiser)
            .ToListAsync();

        return Ok(events);
    }

    /// <summary>Get a single event by id. Both roles.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        if (HttpContext.GetUserId() == null) return Unauthorized();

        var ev = await _db.Events
            .Include(e => e.Categories)
            .Include(e => e.Organiser)
            .FirstOrDefaultAsync(e => e.Id == id);

        return ev == null ? NotFound() : Ok(ev);
    }

    /// <summary>Create an event. Organiser only.</summary>
    [HttpPost]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> Create([FromBody] EventDto dto)
    {
        var ev = new Event
        {
            Name = dto.Name,
            Description = dto.Description,
            EventDate = dto.EventDate,
            Location = dto.Location,
            Distance = dto.Distance,
            EventType = dto.EventType,
            OrganiserId = HttpContext.GetUserId()!.Value
        };

        _db.Events.Add(ev);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = ev.Id }, ev);
    }

    /// <summary>Update an event. Organiser (owner) only.</summary>
    [HttpPut("{id:int}")]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> Update(int id, [FromBody] EventDto dto)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();

        if (ev.OrganiserId != HttpContext.GetUserId())
            return Forbid(); // Not the owner

        ev.Name = dto.Name;
        ev.Description = dto.Description;
        ev.EventDate = dto.EventDate;
        ev.Location = dto.Location;
        ev.Distance = dto.Distance;
        ev.EventType = dto.EventType;

        await _db.SaveChangesAsync();
        return Ok(ev);
    }

    /// <summary>Delete an event. Organiser (owner) only.</summary>
    [HttpDelete("{id:int}")]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return NotFound();

        if (ev.OrganiserId != HttpContext.GetUserId())
            return Forbid(); // Not the owner

        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}