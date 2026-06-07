using System.Text.RegularExpressions;
using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Posts.Commands;

public record CreatePostCommand(string Content, string AuthorId, bool IsLongForm = false, Guid? ParentPostId = null) : IRequest<Guid>;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = new Post
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            AuthorId = request.AuthorId,
            IsLongForm = request.IsLongForm,
            ParentPostId = request.ParentPostId,
            CreatedAt = DateTime.UtcNow
        };

        // Parse Hashtags
        var hashtagMatches = Regex.Matches(request.Content, @"#(\w+)");
        foreach (Match match in hashtagMatches)
        {
            var tagName = match.Groups[1].Value.ToLower();
            var hashtag = await _context.Hashtags.FirstOrDefaultAsync(h => h.Name == tagName, cancellationToken);
            if (hashtag == null)
            {
                hashtag = new Hashtag { Id = Guid.NewGuid(), Name = tagName };
                _context.Hashtags.Add(hashtag);
            }
            entity.PostHashtags.Add(new PostHashtag { Post = entity, Hashtag = hashtag });
        }

        // Parse Mentions
        var mentionMatches = Regex.Matches(request.Content, @"@(\w+)");
        foreach (Match match in mentionMatches)
        {
            var userName = match.Groups[1].Value;
            var mentionedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            if (mentionedUser != null)
            {
                entity.Mentions.Add(new Mention { Post = entity, MentionedUserId = mentionedUser.Id });
            }
        }

        _context.Posts.Add(entity);

        if (request.ParentPostId.HasValue)
        {
            var parent = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.ParentPostId.Value, cancellationToken);
            if (parent != null)
            {
                parent.ReplyCount++;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
