using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;

namespace ScheduleApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ScheduleItem> ScheduleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScheduleItem>()
            .HasIndex(s => new { s.DayOfWeek, s.PeriodNumber })
            .IsUnique()
            .HasDatabaseName("IX_ScheduleItems_DayOfWeek_PeriodNumber");

        modelBuilder.Entity<ScheduleItem>()
            .ToTable("ScheduleItems");

        modelBuilder.Entity<ScheduleItem>().HasData(
            new ScheduleItem 
            { 
                Id = 1, 
                DayOfWeek = "Monday", 
                PeriodNumber = 2, 
                SubjectName = "Numerical Methods",
                CreatedAt = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            new ScheduleItem 
            { 
                Id = 2, 
                DayOfWeek = "Monday", 
                PeriodNumber = 3, 
                SubjectName = "Computer Engineering",
                CreatedAt = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            new ScheduleItem 
            { 
                Id = 3, 
                DayOfWeek = "Monday", 
                PeriodNumber = 5, 
                SubjectName = "Philosophy",
                CreatedAt = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            new ScheduleItem 
            { 
                Id = 4, 
                DayOfWeek = "Wednesday", 
                PeriodNumber = 3, 
                SubjectName = "Numerical Methods",
                CreatedAt = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            new ScheduleItem 
            { 
                Id = 5, 
                DayOfWeek = "Wednesday", 
                PeriodNumber = 4, 
                SubjectName = "Intelligent Programming Systems",
                CreatedAt = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            new ScheduleItem 
            { 
                Id = 6, 
                DayOfWeek = "Wednesday", 
                PeriodNumber = 6, 
                SubjectName = "Numerical Methods",
                CreatedAt = new DateTime(2025, 11, 3, 10, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
