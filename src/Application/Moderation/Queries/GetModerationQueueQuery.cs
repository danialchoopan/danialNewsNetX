using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Moderation.Queries;

public record GetModerationQueueQuery : IRequest<List<Report>>;

public class GetModerationQueueQueryHandler : IRequestHandler<GetModerationQueueQuery, List<Report>>
{
    private readonly IApplicationDbContext _context;

    public GetModerationQueueQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Report>> Handle(GetModerationQueueQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reports
            .Include(r => r.Reporter)
            .Include(r => r.TargetPost)
                .ThenInclude(p => p.Author)
            .Where(r => r.Status == ReportStatus.Pending)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
