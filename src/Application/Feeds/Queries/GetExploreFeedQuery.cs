using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Feeds.Queries;

public record GetExploreFeedQuery(double? CursorScore = null, DateTime? CursorDate = null, int PageSize = 20)
    : IRequest<ExploreFeedResponse>;

public record ExploreFeedResponse(List<PostDto> Posts, double? NextCursorScore, DateTime? NextCursorDate);

public record PostDto(Guid Id, string Content, string AuthorName, string? AuthorPicture, DateTime CreatedAt, int LikeCount, int ReplyCount, bool IsLongForm);

public class GetExploreFeedQueryHandler : IRequestHandler<GetExploreFeedQuery, ExploreFeedResponse>
{
    private readonly IApplicationDbContext _context;

    public GetExploreFeedQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExploreFeedResponse> Handle(GetExploreFeedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .AsQueryable();

        // Cursor-based pagination logic for EngagementScore + CreatedAt
        if (request.CursorScore.HasValue && request.CursorDate.HasValue)
        {
            query = query.Where(p => p.EngagementScore < request.CursorScore.Value ||
                                    (p.EngagementScore == request.CursorScore.Value && p.CreatedAt < request.CursorDate.Value));
        }

        var posts = await query
            .OrderByDescending(p => p.EngagementScore)
            .ThenByDescending(p => p.CreatedAt)
            .Take(request.PageSize)
            .Select(p => new PostDto(
                p.Id,
                p.Content,
                p.Author.UserName ?? "Unknown",
                p.Author.ProfilePictureUrl,
                p.CreatedAt,
                p.LikeCount,
                p.ReplyCount,
                p.IsLongForm))
            .ToListAsync(cancellationToken);

        double? nextScore = null;
        DateTime? nextDate = null;

        if (posts.Count == request.PageSize)
        {
            var lastPost = await _context.Posts.FirstAsync(p => p.Id == posts.Last().Id);
            nextScore = lastPost.EngagementScore;
            nextDate = lastPost.CreatedAt;
        }

        return new ExploreFeedResponse(posts, nextScore, nextDate);
    }
}
