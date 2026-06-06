namespace danialNewsNetX.Domain.Entities;

public class Story
{
    public Guid Id { get; set; }
    public string MediaUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(24);
    public bool IsArchived { get; set; }

    public string AuthorId { get; set; } = null!;
    public virtual AppUser Author { get; set; } = null!;
}
