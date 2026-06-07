using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Moderation.Commands;

public record ReportContentCommand(string ReporterUserId, Guid TargetPostId, string Reason) : IRequest;

public class ReportContentCommandHandler : IRequestHandler<ReportContentCommand>
{
    private readonly IApplicationDbContext _context;

    public ReportContentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ReportContentCommand request, CancellationToken cancellationToken)
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            ReporterUserId = request.ReporterUserId,
            TargetPostId = request.TargetPostId,
            Reason = request.Reason,
            Status = ReportStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record ResolveReportCommand(Guid ReportId, bool HardDelete) : IRequest;

public class ResolveReportCommandHandler : IRequestHandler<ResolveReportCommand>
{
    private readonly IApplicationDbContext _context;

    public ResolveReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ResolveReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.Reports
            .Include(r => r.TargetPost)
            .FirstOrDefaultAsync(r => r.Id == request.ReportId, cancellationToken);

        if (report == null) return;

        if (request.HardDelete && report.TargetPost != null)
        {
            _context.Posts.Remove(report.TargetPost);
            report.Status = ReportStatus.Resolved;
        }
        else
        {
            report.Status = ReportStatus.Dismissed;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
