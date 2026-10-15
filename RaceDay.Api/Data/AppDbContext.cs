// File:    AppDbContext.cs
// Author:  Kieran Pillay — ST10445189
// Date:    October 2026
// Purpose: EF Core DbContext mapping RaceDay entities to SQL Server.
// Attribution:
//   - Base DbContext pattern: Microsoft Learn, "DbContext Configuration":
//     https://learn.microsoft.com/ef/core/dbcontext-configuration/
//   - Relationships: Microsoft Learn, "Relationships":
//     https://learn.microsoft.com/ef/core/modeling/relationships
//   - Cascade delete behaviour: Microsoft Learn, "Cascade Delete":
//     https://learn.microsoft.com/ef/core/saving/cascade-delete
//   - Entity-specific rules derived from the author's Part 1 ERD.


using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;

namespace RaceDay.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();

    /// <summary>
    /// Configures keys, indexes, column constraints, and relationships.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder mb)
    {
        // User — Email must be unique (login identifier).
        // Role is stored as a string ("Organiser" / "Participant").
        mb.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
            e.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            e.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            e.Property(u => u.Email).HasMaxLength(255).IsRequired();
            e.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();
        });

        // Event — belongs to an Organiser. Restrict delete so removing a User
        // does not silently delete their events.
        mb.Entity<Event>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Location).HasMaxLength(200).IsRequired();
            e.Property(x => x.Distance).HasPrecision(6, 2);
            e.Property(x => x.EventType).HasConversion<string>().HasMaxLength(10);

            e.HasOne(x => x.Organiser)
             .WithMany()
             .HasForeignKey(x => x.OrganiserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Category — belongs to an Event. Cascade delete so removing an Event
        // also removes its categories.
        mb.Entity<Category>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasOne(x => x.Event)
             .WithMany(ev => ev.Categories)
             .HasForeignKey(x => x.EventId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Enrolment — links Participant + Event + Category.
        // Unique composite index prevents the same participant from enrolling
        // twice in the same event.
        //
        // IMPORTANT: Event FK uses Restrict (not Cascade) to avoid the SQL Server
        // "multiple cascade paths" error. Cascade already flows through
        // Event -> Category -> Enrolment, so a second direct cascade would be
        // rejected by SQL Server.
        mb.Entity<Enrolment>(e =>
        {
            e.HasIndex(x => new { x.ParticipantId, x.EventId }).IsUnique();

            e.HasOne(x => x.Participant)
             .WithMany()
             .HasForeignKey(x => x.ParticipantId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Event)
             .WithMany(ev => ev.Enrolments)
             .HasForeignKey(x => x.EventId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Category)
             .WithMany()
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Result — one-to-one with Enrolment.
        // Unique index on EnrolmentId enforces the one-to-one cardinality.
        mb.Entity<Result>(e =>
        {
            e.HasIndex(x => x.EnrolmentId).IsUnique();
            e.HasOne(x => x.Enrolment)
             .WithOne()
             .HasForeignKey<Result>(x => x.EnrolmentId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}