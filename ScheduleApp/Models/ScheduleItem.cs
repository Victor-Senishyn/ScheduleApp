using System.ComponentModel.DataAnnotations;

namespace ScheduleApp.Models;

public class ScheduleItem
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Day of week is required")]
    public string DayOfWeek { get; set; } = string.Empty;

    [Required(ErrorMessage = "Period number is required")]
    [Range(1, 7, ErrorMessage = "Period number must be between 1 and 7")]
    public int PeriodNumber { get; set; }

    [Required(ErrorMessage = "Subject name is required")]
    [StringLength(200, ErrorMessage = "Subject name cannot exceed 200 characters")]
    public string SubjectName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

