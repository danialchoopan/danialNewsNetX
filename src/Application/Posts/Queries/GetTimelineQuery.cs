using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Application.Common.Models;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Posts.Queries;

public record GetTimelineQuery(string UserId, int PageSize = 20, string? Cursor = null) : IRequest<PaginatedList<PostDto>>;

public class PostDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string AuthorUsername { get; set; } = null!;
    public string? AuthorProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public int RepliesCount { get; set; }
}

public class GetTimelineQueryHandler : IRequestHandler<GetTimelineQuery, PaginatedList<PostDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTimelineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PostDto>> Handle(GetTimelineQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Cursor))
        {
            if (DateTime.TryParse(request.Cursor, out var cursorDate))
            {
                query = query.Where(p => p.CreatedAt < cursorDate);
            }
        }

        var posts = await query
            .Take(request.PageSize + 1)
            .Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                AuthorName = p.Author.UserName ?? "", // Placeholder
                AuthorUsername = p.Author.UserName ?? "",
                AuthorProfilePictureUrl = p.Author.ProfilePictureUrl,
                CreatedAt = p.CreatedAt,
                LikesCount = p.Likes.Count,
                RepliesCount = p.Replies.Count
            })
            .ToListAsync(cancellationToken);

        string? nextCursor = null;
        if (posts.Count > request.PageSize)
        {
            nextCursor = posts[request.PageSize - 1].CreatedAt.ToString("o");
            posts = posts.Take(request.PageSize).ToList();
        }

        return new PaginatedList<PostDto>(posts, nextCursor);
    }
}
