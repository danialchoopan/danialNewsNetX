using danialNewsNetX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AppUser> Users { get; }
    DbSet<Post> Posts { get; }
    DbSet<Follow> Follows { get; }
    DbSet<Story> Stories { get; }
    DbSet<Like> Likes { get; }
    DbSet<Hashtag> Hashtags { get; }
    DbSet<PostHashtag> PostHashtags { get; }
    DbSet<Mention> Mentions { get; }
    DbSet<SystemFeature> SystemFeatures { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
