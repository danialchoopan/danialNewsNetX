namespace danialNewsNetX.Domain.Entities;

public class Like
{
    public string UserId { get; set; } = null!;
    public virtual AppUser User { get; set; } = null!;

    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
