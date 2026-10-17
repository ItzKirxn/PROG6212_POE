// File:    JarredLovesTests.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: The legendary Jarred Loves is our canonical Organiser persona.
//          These tests cover registration, login, event management, role
//          restrictions, and enrolments.


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Controllers;
using RaceDay.Api.DTOs;
using RaceDay.Api.Models;
using RaceDay.Api.Services;
using Xunit;

namespace RaceDay.Api.Tests;

public class JarredLovesTests
{
    private const string JarredEmail = "jarred.loves@raceday.co.za";
    private const string JarredPassword = "L0vesRunning!";

    // ------------------------------------------------------------------------
    // 1. AUTHENTICATION TESTS
    // ------------------------------------------------------------------------

    [Fact]
    public async Task JarredLoves_Can_Register_As_Organiser()
    {
        var db = TestSetup.CreateDb();
        var ctrl = new AuthController(db, new PasswordHasherService()) { ControllerContext = TestSetup.NoSession() };

        var result = await ctrl.Register(new RegisterDto("Jarred", "Loves", JarredEmail, JarredPassword, UserRole.Organiser));

        Assert.IsType<OkObjectResult>(result);
        var user = db.Users.Single();
        Assert.Equal("Jarred", user.FirstName);
        Assert.Equal(UserRole.Organiser, user.Role);
        Assert.NotEqual(JarredPassword, user.PasswordHash); // Prove password is hashed!
    }

    [Fact]
    public async Task JarredLoves_Cannot_Register_With_Duplicate_Email()
    {
        var db = TestSetup.CreateDb();
        var ctrl = new AuthController(db, new PasswordHasherService()) { ControllerContext = TestSetup.NoSession() };

        await ctrl.Register(new RegisterDto("Jarred", "Loves", JarredEmail, JarredPassword, UserRole.Organiser));
        var secondResult = await ctrl.Register(new RegisterDto("Jarred", "Loves", JarredEmail, JarredPassword, UserRole.Organiser));

        Assert.IsType<ConflictObjectResult>(secondResult);
    }

    [Fact]
    public async Task JarredLoves_Can_Login_With_Correct_Password()
    {
        var db = TestSetup.CreateDb();
        var ctrl = new AuthController(db, new PasswordHasherService()) { ControllerContext = TestSetup.NoSession() };

        await ctrl.Register(new RegisterDto("Jarred", "Loves", JarredEmail, JarredPassword, UserRole.Organiser));
        var loginResult = await ctrl.Login(new LoginDto(JarredEmail, JarredPassword));

        Assert.IsType<OkObjectResult>(loginResult);
        Assert.Equal(1, ctrl.HttpContext.Session.GetInt32("UserId"));
        Assert.Equal("Organiser", ctrl.HttpContext.Session.GetString("Role"));
    }

    [Fact]
    public async Task JarredLoves_Login_Fails_With_Wrong_Password()
    {
        var db = TestSetup.CreateDb();
        var ctrl = new AuthController(db, new PasswordHasherService()) { ControllerContext = TestSetup.NoSession() };

        await ctrl.Register(new RegisterDto("Jarred", "Loves", JarredEmail, JarredPassword, UserRole.Organiser));
        var loginResult = await ctrl.Login(new LoginDto(JarredEmail, "WrongPassword!"));

        Assert.IsType<UnauthorizedObjectResult>(loginResult);
    }

    // ------------------------------------------------------------------------
    // 2. EVENT MANAGEMENT TESTS
    // ------------------------------------------------------------------------

    [Fact]
    public async Task JarredLoves_Can_Create_An_Event()
    {
        var db = TestSetup.CreateDb();
        var jarred = new User { FirstName = "Jarred", LastName = "Loves", Email = JarredEmail, PasswordHash = "x", Role = UserRole.Organiser };
        db.Users.Add(jarred);
        await db.SaveChangesAsync();

        var ctrl = new EventsController(db) { ControllerContext = TestSetup.WithSession(UserRole.Organiser, jarred.Id) };

        var result = await ctrl.Create(new EventDto("Comrades", "Ultra", DateTime.Now.AddDays(10), "PMB", 87.0m, EventType.Run));

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Single(db.Events);
        Assert.Equal("Comrades", db.Events.First().Name);
    }

    // ------------------------------------------------------------------------
    // 3. EVENT ENROLMENT TESTS
    // ------------------------------------------------------------------------

    [Fact]
    public async Task Participant_Can_Enrol_And_Record_Is_Created()
    {
        var db = TestSetup.CreateDb();

        // Setup Jarred (Organiser) and an Event
        var jarred = new User { Role = UserRole.Organiser, FirstName = "Jarred", Email = "j@x.com", LastName = "Loves", PasswordHash = "x" };

        var ev = new Event
        {
            Name = "Comrades",
            Organiser = jarred,
            EventDate = DateTime.Now,
            Location = "PMB",
            Distance = 87.0m,
            EventType = EventType.Run
        };

        // Setup Participant
        var sipho = new User { Role = UserRole.Participant, FirstName = "Sipho", Email = "s@x.com", LastName = "Nkosi", PasswordHash = "x" };
        var category = new Category { Name = "Senior", Event = ev };

        db.Users.AddRange(jarred, sipho);
        db.Events.Add(ev);
        db.Categories.Add(category);
        await db.SaveChangesAsync();

        var ctrl = new EnrolmentsController(db) { ControllerContext = TestSetup.WithSession(UserRole.Participant, sipho.Id) };

        var result = await ctrl.Enrol(new EnrolDto(ev.Id, category.Id));

        Assert.IsType<OkObjectResult>(result);
        Assert.Single(db.Enrolments);
        Assert.Equal(sipho.Id, db.Enrolments.First().ParticipantId);
        Assert.Equal(ev.Id, db.Enrolments.First().EventId);
    }
}