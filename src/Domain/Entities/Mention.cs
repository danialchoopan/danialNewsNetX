namespace danialNewsNetX.Domain.Entities;

public class Mention
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;

    public string MentionedUserId { get; set; } = null!;
    public virtual AppUser MentionedUser { get; set; } = null!;
}
