using danialNewsNetX.WebUI.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace danialNewsNetX.WebUI.Controllers;

public class SendAnnouncementCommandHandler : IRequestHandler<SendAnnouncementCommand, Unit>
{
    private readonly IHubContext<AdminHub> _hubContext;

    public SendAnnouncementCommandHandler(IHubContext<AdminHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task<Unit> Handle(SendAnnouncementCommand request, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveAnnouncement", request.Message, cancellationToken);
        return Unit.Value;
    }
}
