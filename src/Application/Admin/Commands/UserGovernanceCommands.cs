using danialNewsNetX.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace danialNewsNetX.Application.Admin.Commands;

public record UpdateUserRoleCommand(string UserId, string RoleName) : IRequest;

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserRoleCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) return;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, request.RoleName);
    }
}

public record ToggleUserVerificationCommand(string UserId) : IRequest;

public class ToggleUserVerificationCommandHandler : IRequestHandler<ToggleUserVerificationCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public ToggleUserVerificationCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ToggleUserVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) return;

        user.IsVerified = !user.IsVerified;
        await _userManager.UpdateAsync(user);
    }
}

public record MuteUserCommand(string UserId, bool Mute) : IRequest;

public class MuteUserCommandHandler : IRequestHandler<MuteUserCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public MuteUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(MuteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) return;

        user.IsMuted = request.Mute;
        await _userManager.UpdateAsync(user);
    }
}

public record BanUserCommand(string UserId, int? Days = null) : IRequest;

public class BanUserCommandHandler : IRequestHandler<BanUserCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public BanUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) return;

        if (request.Days.HasValue)
        {
            user.BanExpiresAt = DateTime.UtcNow.AddDays(request.Days.Value);
            user.LockoutEnd = user.BanExpiresAt;
        }
        else
        {
            user.BanExpiresAt = DateTime.MaxValue;
            user.LockoutEnd = DateTimeOffset.MaxValue;
        }

        await _userManager.UpdateAsync(user);
    }
}
