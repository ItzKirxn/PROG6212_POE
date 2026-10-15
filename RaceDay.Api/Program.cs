// File:    Program.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: Entry point for the RaceDay RESTful API. Configures:
//           - EF Core with SQL Server (LocalDB)
//           - Session-based authentication
//           - Password hashing service
//           - Swagger / OpenAPI documentation
//           - Automatic EF Core migrations on startup
// Attribution:
//   - DbContext configuration:
//     https://learn.microsoft.com/ef/core/dbcontext-configuration/
//   - Session state:
//     https://learn.microsoft.com/aspnet/core/fundamentals/app-state
//   - Swagger setup:
//     https://learn.microsoft.com/aspnet/core/tutorials/getting-started-with-swashbuckle
//   - Auto-migration on startup:
//     https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying
//   - Integration test support (partial Program class):
//     https://learn.microsoft.com/aspnet/core/test/integration-tests


using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------------
// 1. Database — EF Core with SQL Server LocalDB.
// Connection string is read from appsettings.json -> "ConnectionStrings:Default".
// ----------------------------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ----------------------------------------------------------------------------
// 2. Application services — password hashing used by AuthController.
// ----------------------------------------------------------------------------
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

// ----------------------------------------------------------------------------
// 3. Session state — keeps users logged in across requests.
// UserId and Role are stored in the session and read by SessionExtensions
// and RequireRoleAttribute to enforce role-based access.
// ----------------------------------------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".RaceDay.Session";
});

// ----------------------------------------------------------------------------
// 4. MVC controllers + Swagger.
// XML doc comments from generated .xml file appear as descriptions in Swagger.
// ----------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "RaceDay API",
        Version = "v1",
        Description = "RESTful API for the RaceDay event management system."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ----------------------------------------------------------------------------
// 5. Apply EF Core migrations at startup so the database schema is always
// up to date when the API starts.
// ----------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ----------------------------------------------------------------------------
// 6. HTTP request pipeline.
// Session middleware must run before authorization so the Role stored in
// session is available to controller action filters.
// ----------------------------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI();

app.UseSession();
app.UseAuthorization();
app.MapControllers();

app.Run();

// ----------------------------------------------------------------------------
// Public partial class so integration tests can use WebApplicationFactory<Program>.
// ----------------------------------------------------------------------------
public partial class Program { }