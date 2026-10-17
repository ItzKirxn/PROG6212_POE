// File:    TestSetup.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Helpers for setting up an in-memory database and fake HTTP session
//          for testing controllers directly.


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;

namespace RaceDay.Api.Tests;

public static class TestSetup
{
    public static AppDbContext CreateDb()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(opts);
    }

    public static ControllerContext WithSession(UserRole role, int userId)
    {
        var ctx = new DefaultHttpContext { Session = new FakeSession() };
        ctx.Session.SetInt32("UserId", userId);
        ctx.Session.SetString("Role", role.ToString());
        return new ControllerContext { HttpContext = ctx };
    }

    public static ControllerContext NoSession()
    {
        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext { Session = new FakeSession() }
        };
    }
}

public class FakeSession : ISession
{
    private readonly Dictionary<string, byte[]> _sessionStorage = new();

    public bool IsAvailable => true;
    public string Id => Guid.NewGuid().ToString();
    public IEnumerable<string> Keys => _sessionStorage.Keys;

    public void Clear() => _sessionStorage.Clear();
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Remove(string key) => _sessionStorage.Remove(key);
    public void Set(string key, byte[] value) => _sessionStorage[key] = value;
    public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value!);
}