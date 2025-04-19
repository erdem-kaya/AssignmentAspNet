using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;


// Den nuvarande kodstrukturen för NotificationService är för enkel jämfört med den struktur jag har byggt hittills, så jag gillar inte denna struktur - jag vill bygga NotificationRepository, Model etc. struktur. Jag kommer att bygga det senare.

// I framtiden behöver jag varna administratörer och användare separat i NotificationService, denna struktur är otillräcklig!... 

public interface INotificationService
{
    Task AddNotificationAsync(int notificationTypeId, string message, string image = null!, int notificationTargetGroup = 1);
    Task DismissNotificationAsync(string notificationId, string userId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);
}

public class NotificationService(DataContext context) : INotificationService
{
    private readonly DataContext _context = context;

    public async Task AddNotificationAsync(int notificationTypeId, string message, string image = null!, int notificationTargetGroup = 1)
    {
        if (string.IsNullOrEmpty(image))
        {
            switch (notificationTypeId)
            {
                case 1:
                    image = "~/WebApp/wwwroot/Images/avatar-default.svg";
                    break;
                case 2:
                    image = "~/WebApp/wwwroot/Images/project-default.svg";
                    break;
            }
        }

        var notification = new NotificationEntity
        {
            NotificationTypeId = notificationTypeId,
            TargetGroupId = notificationTargetGroup,
            Message = message,
            Icon = image,
            CreatedAt = DateTime.UtcNow,

        };

        _context.Add(notification);
        await _context.SaveChangesAsync();
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
                NotificationId = notificationId,
                UserId = userId,
            };
            _context.Add(dismissed);
            await _context.SaveChangesAsync();
        }
    }
}
