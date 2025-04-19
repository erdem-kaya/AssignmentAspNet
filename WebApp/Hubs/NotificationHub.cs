using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object allNotifiction)
    {
       await Clients.All.SendAsync("AllReceiveNotification", allNotifiction);
    }

    public async Task SendNotificationToAdmins(object adminNotifiction)
    {
        await Clients.All.SendAsync("AdminReceiveNotification", adminNotifiction);
    }

}
