using Microsoft.AspNetCore.SignalR;

namespace danialNewsNetX.WebUI.Hubs;

public class AdminHub : Hub
{
    public async Task SendAnnouncement(string message)
    {
        await Clients.All.SendAsync("ReceiveAnnouncement", message);
    }
}
