using danialNewsNetX.Application.Admin.Commands;
using danialNewsNetX.Application.Admin.Queries;
using danialNewsNetX.Application.Moderation.Commands;
using danialNewsNetX.Application.Moderation.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace danialNewsNetX.WebUI.Controllers;

[Authorize(Roles = "SuperAdmin")]
[Route("admin/super")]
public class SuperAdminController : Controller
{
    private readonly IMediator _mediator;

    public SuperAdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var telemetry = await _mediator.Send(new GetSystemTelemetryQuery());
        return View(telemetry);
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return View(users);
    }

    [HttpPost("verify/{userId}")]
    public async Task<IActionResult> ToggleVerification(string userId)
    {
        await _mediator.Send(new ToggleUserVerificationCommand(userId));
        return RedirectToAction(nameof(Users));
    }

    [HttpPost("ban/{userId}")]
    public async Task<IActionResult> BanUser(string userId)
    {
        await _mediator.Send(new BanUserCommand(userId, 30));
        return RedirectToAction(nameof(Users));
    }

    [HttpPost("mute/{userId}")]
    public async Task<IActionResult> ToggleMute(string userId, bool mute)
    {
        await _mediator.Send(new MuteUserCommand(userId, mute));
        return RedirectToAction(nameof(Users));
    }

    [HttpPost("role/{userId}")]
    public async Task<IActionResult> UpdateRole(string userId, string role)
    {
        await _mediator.Send(new UpdateUserRoleCommand(userId, role));
        return RedirectToAction(nameof(Users));
    }

    [HttpGet("moderation")]
    public async Task<IActionResult> Moderation()
    {
        var reports = await _mediator.Send(new GetModerationQueueQuery());
        return View(reports);
    }

    [HttpPost("moderation/resolve/{reportId}")]
    public async Task<IActionResult> ResolveReport(Guid reportId, bool hardDelete)
    {
        await _mediator.Send(new ResolveReportCommand(reportId, hardDelete));
        return RedirectToAction(nameof(Moderation));
    }

    [HttpPost("announcement")]
    public async Task<IActionResult> SendAnnouncement(string message)
    {
        await _mediator.Send(new SendAnnouncementCommand(message));
        return RedirectToAction(nameof(Index));
    }
}

public record SendAnnouncementCommand(string Message) : IRequest<Unit>;
