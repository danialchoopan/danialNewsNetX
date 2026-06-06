using danialNewsNetX.Application.Common.Interfaces;
using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Application.Admin.Queries;

public record GetUsersQuery : IRequest<List<AppUser>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<AppUser>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .OrderBy(u => u.UserName)
            .Take(50)
            .ToListAsync(cancellationToken);
    }
}
