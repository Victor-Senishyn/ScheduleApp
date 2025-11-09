using Microsoft.EntityFrameworkCore;
using ScheduleApp.Data;
using ScheduleApp.Models;

namespace ScheduleApp.Services;

public class ScheduleService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<ScheduleService> _logger;

    public ScheduleService(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        ILogger<ScheduleService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<List<ScheduleItem>> GetAllScheduleItemsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ScheduleItems
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.PeriodNumber)
            .ToListAsync();
    }

    public async Task<List<ScheduleItem>> GetScheduleItemsByDayAsync(string dayOfWeek)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ScheduleItems
            .Where(s => s.DayOfWeek == dayOfWeek)
            .OrderBy(s => s.PeriodNumber)
            .ToListAsync();
    }

    public async Task<ScheduleItem?> GetScheduleItemByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ScheduleItems.FindAsync(id);
    }

    public async Task<(bool Success, string Message)> AddScheduleItemAsync(ScheduleItem item)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var exists = await context.ScheduleItems
                .AnyAsync(s => s.DayOfWeek == item.DayOfWeek && s.PeriodNumber == item.PeriodNumber);

            if (exists)
            {
                return (false, $"Period {item.PeriodNumber} on {item.DayOfWeek} already exists!");
            }

            item.CreatedAt = DateTime.UtcNow;
            context.ScheduleItems.Add(item);
            await context.SaveChangesAsync();

            _logger.LogInformation("Added schedule: {Day}, Period {Period}, {Subject}", 
                item.DayOfWeek, item.PeriodNumber, item.SubjectName);

            return (true, "Schedule added successfully!");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error adding schedule");
            return (false, "Failed to add schedule. Entry may already exist.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding schedule");
            return (false, "An error occurred while adding schedule.");
        }
    }

    public async Task<(bool Success, string Message)> UpdateScheduleItemAsync(ScheduleItem item)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var existingItem = await context.ScheduleItems.FindAsync(item.Id);
            if (existingItem == null)
            {
                return (false, "Schedule not found!");
            }

            var duplicate = await context.ScheduleItems
                .AnyAsync(s => s.Id != item.Id && 
                              s.DayOfWeek == item.DayOfWeek && 
                              s.PeriodNumber == item.PeriodNumber);

            if (duplicate)
            {
                return (false, $"Period {item.PeriodNumber} on {item.DayOfWeek} is already occupied!");
            }

            existingItem.DayOfWeek = item.DayOfWeek;
            existingItem.PeriodNumber = item.PeriodNumber;
            existingItem.SubjectName = item.SubjectName;
            existingItem.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            _logger.LogInformation("Updated schedule ID {Id}: {Day}, Period {Period}, {Subject}", 
                item.Id, item.DayOfWeek, item.PeriodNumber, item.SubjectName);

            return (true, "Schedule updated successfully!");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating schedule");
            return (false, "Failed to update schedule. Entry may already exist.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating schedule");
            return (false, "An error occurred while updating schedule.");
        }
    }

    public async Task<(bool Success, string Message)> DeleteScheduleItemAsync(int id)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var item = await context.ScheduleItems.FindAsync(id);
            if (item == null)
            {
                return (false, "Schedule not found!");
            }

            context.ScheduleItems.Remove(item);
            await context.SaveChangesAsync();

            _logger.LogInformation("Deleted schedule ID {Id}: {Day}, Period {Period}, {Subject}", 
                id, item.DayOfWeek, item.PeriodNumber, item.SubjectName);

            return (true, "Schedule deleted successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting schedule");
            return (false, "An error occurred while deleting schedule.");
        }
    }

    public async Task<bool> ExistsAsync(string dayOfWeek, int periodNumber)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ScheduleItems
            .AnyAsync(s => s.DayOfWeek == dayOfWeek && s.PeriodNumber == periodNumber);
    }
}
