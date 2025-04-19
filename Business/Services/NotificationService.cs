using Business.Hubs;
using Business.Models.UserProfile;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;


// Den nuvarande kodstrukturen för NotificationService är för enkel jämfört med den struktur jag har byggt hittills, så jag gillar inte denna struktur - jag vill bygga NotificationRepository, Model etc. struktur. Jag kommer att bygga det senare.

// I framtiden behöver jag varna administratörer och användare separat i NotificationService, denna struktur är otillräcklig!... 

public interface INotificationService
{
    Task AddNotificationAsync(NotificationEntity notificationEntity, string userId = "anonymous");
    Task DismissNotificationAsync(string notificationId, string userId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);
}

public class NotificationService(DataContext context, IHubContext<NotificationHub> notificationHub) : INotificationService
{
    private readonly DataContext _context = context;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

    public async Task AddNotificationAsync(NotificationEntity notificationEntity, string userId = "anonymous")
    {
        if (string.IsNullOrEmpty(notificationEntity.Icon))
        {
            switch (notificationEntity.NotificationTypeId)
            {
                case 1:
                    notificationEntity.Icon = "/Images/avatar-default.svg";
                    break;
                case 2:
                    notificationEntity.Icon = "/Images/project-default.svg";
                    break;
            }
        }

        _context.Add(notificationEntity);
        await _context.SaveChangesAsync();

        var notification = await GetNotificationsAsync(userId);
        var newNotification = notification.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

        if (newNotification != null)
        {
            await _notificationHub.Clients.All.SendAsync("AllReceiveNotification", newNotification);
        }
    }


    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10)
    {
        var dismissedIds = await _context.DismissedNotifications
             .Where(x => x.UserId == userId)
             .Select(x => x.NotificationId)
             .ToListAsync();

        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync();

        return notifications;
    }

    public async Task DismissNotificationAsync(string notificationId, string userId)
    {
        var allreadyDismissed = await _context.DismissedNotifications
            .AnyAsync(x => x.NotificationId == notificationId && x.UserId == userId);

        if (!allreadyDismissed)
        {
            var dismissed = new NotificationDismissedEntity
            {
                UserId = userId,
                NotificationId = notificationId, 
            };
            _context.Add(dismissed);
            await _context.SaveChangesAsync();
        }

    }
}
