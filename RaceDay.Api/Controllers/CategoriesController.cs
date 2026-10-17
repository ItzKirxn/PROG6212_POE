// File:    CategoriesController.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Handles viewing, creating, and deleting event categories.
//          Organisers can manage; both roles can view.


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Helpers;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    /// <summary>List categories for an event. Both roles.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(int eventId)
    {
        if (HttpContext.GetUserId() == null) return Unauthorized();

        var cats = await _db.Categories.Where(c => c.EventId == eventId).ToListAsync();
        return Ok(cats);
    }

    /// <summary>Add a category to an event. Organiser (owner) only.</summary>
    [HttpPost]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> Create(int eventId, [FromBody] CategoryDto dto)
    {
        var ev = await _db.Events.FindAsync(eventId);
        if (ev == null) return NotFound("Event not found.");

        if (ev.OrganiserId != HttpContext.GetUserId()) return Forbid();

        var cat = new Category { Name = dto.Name, EventId = eventId };
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();

        return Ok(cat);
    }

    /// <summary>Delete a category. Organiser (owner) only.</summary>
    [HttpDelete("/api/categories/{id:int}")]
    [RequireRole(UserRole.Organiser)]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _db.Categories.Include(c => c.Event).FirstOrDefaultAsync(c => c.Id == id);
        if (cat == null) return NotFound();

        if (cat.Event.OrganiserId != HttpContext.GetUserId()) return Forbid();

        _db.Categories.Remove(cat);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}