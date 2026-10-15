// ============================================================================
// File:    PasswordHasherService.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Wraps ASP.NET Core Identity's PasswordHasher so the API can hash
//          and verify passwords without depending on full Identity.
// Attribution: https://learn.microsoft.com/aspnet/core/security/data-protection/consumer-apis/password-hashing
// ============================================================================

using Microsoft.AspNetCore.Identity;

namespace RaceDay.Api.Services;

public interface IPasswordHasherService
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password) => _hasher.HashPassword(null!, password);

    public bool Verify(string password, string hash) =>
        _hasher.VerifyHashedPassword(null!, hash, password)
            != PasswordVerificationResult.Failed;
}