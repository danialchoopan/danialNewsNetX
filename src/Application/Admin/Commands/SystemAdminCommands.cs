using danialNewsNetX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Admin.Commands;

public record UpdateFeatureToggleCommand(string Key, bool IsEnabled) : IRequest;

public class UpdateFeatureToggleCommandHandler : IRequestHandler<UpdateFeatureToggleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFeatureToggleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateFeatureToggleCommand request, CancellationToken cancellationToken)
    {
        var feature = await _context.SystemFeatures.FirstOrDefaultAsync(f => f.Key == request.Key, cancellationToken);
        if (feature != null)
        {
            feature.IsEnabled = request.IsEnabled;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

public record DeleteContentCommand(Guid PostId) : IRequest;

public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteContentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteContentCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
