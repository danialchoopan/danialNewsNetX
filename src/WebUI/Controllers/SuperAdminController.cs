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

    [HttpPost("announcement")]
    public async Task<IActionResult> SendAnnouncement(string message)
    {
        await _mediator.Send(new SendAnnouncementCommand(message));
        return RedirectToAction(nameof(Index));
    }
}

public record SendAnnouncementCommand(string Message) : IRequest;
// Handler will be implemented in SignalR step
