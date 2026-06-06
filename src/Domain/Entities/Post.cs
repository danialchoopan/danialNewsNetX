namespace danialNewsNetX.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsLongForm { get; set; }

    public string AuthorId { get; set; } = null!;
    public virtual AppUser Author { get; set; } = null!;

    public Guid? ParentPostId { get; set; }
    public virtual Post? ParentPost { get; set; }

    public virtual ICollection<Post> Replies { get; set; } = new List<Post>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    public virtual ICollection<Mention> Mentions { get; set; } = new List<Mention>();

    // Repost/Quote Tweet logic
    public Guid? OriginalPostId { get; set; }
    public virtual Post? OriginalPost { get; set; }
    public bool IsRepost { get; set; }
}
