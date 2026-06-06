namespace danialNewsNetX.Domain.Entities;

public class Hashtag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
}

public class PostHashtag
{
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;

    public Guid HashtagId { get; set; }
    public virtual Hashtag Hashtag { get; set; } = null!;
}
