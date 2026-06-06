namespace danialNewsNetX.Domain.Entities;

public class Follow
{
    public string ObserverId { get; set; } = null!;
    public virtual AppUser Observer { get; set; } = null!;

    public string TargetId { get; set; } = null!;
    public virtual AppUser Target { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
