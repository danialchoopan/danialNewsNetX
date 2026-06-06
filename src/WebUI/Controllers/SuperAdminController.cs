using danialNewsNetX.Application.Admin.Commands;
using danialNewsNetX.Application.Admin.Queries;
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

    public async Task<IActionResult> Index()
    {
        var telemetry = await _mediator.Send(new GetSystemTelemetryQuery());
        return View(telemetry);
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        // Simple implementation to show list of users for moderation
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
        await _mediator.Send(new BanUserCommand(userId, 30)); // Default 30 days
        return RedirectToAction(nameof(Users));
    }

    [HttpPost("announcement")]
    public async Task<IActionResult> SendAnnouncement(string message)
    {
        await _mediator.Send(new SendAnnouncementCommand(message));
        return RedirectToAction(nameof(Index));
    }
}

public record SendAnnouncementCommand(string Message) : IRequest;
// Handler will be implemented in SignalR step
