using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Story> Stories => Set<Story>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Hashtag> Hashtags => Set<Hashtag>();
    public DbSet<PostHashtag> PostHashtags => Set<PostHashtag>();
    public DbSet<Mention> Mentions => Set<Mention>();
    public DbSet<SystemFeature> SystemFeatures => Set<SystemFeature>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Follow>(entity =>
        {
            entity.HasKey(f => new { f.ObserverId, f.TargetId });

            entity.HasOne(f => f.Observer)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.Target)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.TargetId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Post>(entity =>
        {
            entity.HasIndex(p => p.CreatedAt);
            entity.HasIndex(p => p.AuthorId);
            entity.HasIndex(p => new { p.AuthorId, p.CreatedAt }).IsDescending(false, true);

            // Optimization for Trending Feed
            entity.HasIndex(p => new { p.EngagementScore, p.CreatedAt }).IsDescending(true, true);
            entity.HasIndex(p => new { p.LikeCount, p.CreatedAt }).IsDescending(true, true);

            entity.HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId);

            entity.HasOne(p => p.ParentPost)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentPostId);
        });

        builder.Entity<Like>(entity =>
        {
            entity.HasKey(l => new { l.UserId, l.PostId });

            entity.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);

            entity.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);
        });

        builder.Entity<PostHashtag>(entity =>
        {
            entity.HasKey(ph => new { ph.PostId, ph.HashtagId });
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
