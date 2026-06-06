using Microsoft.AspNetCore.Identity;

namespace danialNewsNetX.Domain.Entities;

public class AppUser : IdentityUser
{
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? HeaderBannerUrl { get; set; }
    public string? HexThemeColor { get; set; }
    public bool IsVerified { get; set; }
    public bool IsMuted { get; set; }
    public DateTime? BanExpiresAt { get; set; }

    public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public virtual ICollection<Follow> Following { get; set; } = new List<Follow>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
}
