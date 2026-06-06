using danialNewsNetX.WebUI.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace danialNewsNetX.WebUI.Controllers;

public class SendAnnouncementCommandHandler : IRequestHandler<SendAnnouncementCommand>
{
    private readonly IHubContext<AdminHub> _hubContext;

    public SendAnnouncementCommandHandler(IHubContext<AdminHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(SendAnnouncementCommand request, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveAnnouncement", request.Message, cancellationToken);
    }
}
