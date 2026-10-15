// File:    AuthDtos.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Request/response data-transfer objects for authentication.
// Attribution: DTO pattern — Microsoft Learn, "Create web APIs with ASP.NET Core":
//   https://learn.microsoft.com/aspnet/core/web-api/


using RaceDay.Api.Models;

namespace RaceDay.Api.DTOs;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role);

public record LoginDto(string Email, string Password);

public record UpdateProfileDto(string FirstName, string LastName);