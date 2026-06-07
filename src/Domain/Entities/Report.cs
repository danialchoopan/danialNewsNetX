namespace danialNewsNetX.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    public string ReporterUserId { get; set; } = null!;
    public virtual AppUser Reporter { get; set; } = null!;

    public Guid? TargetPostId { get; set; }
    public virtual Post? TargetPost { get; set; }

    public string Reason { get; set; } = null!; // e.g. Spam, Hate Speech, etc.
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum ReportStatus
{
    Pending,
    Resolved,
    Dismissed
}
