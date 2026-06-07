using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Posts.Commands;

public record ToggleLikeCommand(string UserId, Guid PostId) : IRequest;

public class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand>
{
    private readonly IApplicationDbContext _context;

    public ToggleLikeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);
        if (post == null) return;

        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.PostId == request.PostId, cancellationToken);

        if (existingLike != null)
        {
            _context.Likes.Remove(existingLike);
            post.LikeCount = Math.Max(0, post.LikeCount - 1);
        }
        else
        {
            _context.Likes.Add(new Like { UserId = request.UserId, PostId = request.PostId });
            post.LikeCount++;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
