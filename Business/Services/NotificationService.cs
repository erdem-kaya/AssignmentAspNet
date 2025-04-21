using Business.Hubs;
using Business.Models.UserProfile;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Business.Services;


// Den nuvarande kodstrukturen för NotificationService är för enkel jämfört med den struktur jag har byggt hittills, så jag gillar inte denna struktur - jag vill bygga NotificationRepository, Model etc. struktur. Jag kommer att bygga det senare.

// I framtiden behöver jag varna administratörer och användare separat i NotificationService, denna struktur är otillräcklig!... 

public interface INotificationService
{
    Task AddNotificationAsync(NotificationEntity notificationEntity, string userId = "anonymous");
    Task DismissNotificationAsync(string notificationId, string userId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);
    Task<List<string>> GetAdminUserIdsAsync();
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

        var adminsNotification = await _context.Notifications
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        if (adminsNotification != null) 
        {
            var adminUserIds = await GetAdminUserIdsAsync();
            foreach (var adminUserId in adminUserIds)
            {

                await _notificationHub.Clients.User(adminUserId).SendAsync("AdminReceiveNotification", new
                {
                    id = adminsNotification.Id,
                    icon = adminsNotification.Icon,
                    message = adminsNotification.Message,
                    created = adminsNotification.CreatedAt,
                    notificationTypeId = adminsNotification.NotificationTypeId
                });
            }
        }     
    }


    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10)
    {
        //ChatGpt hjälpte mig med denna kod

        var userRoles = await _context.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .ToListAsync();

        var isAdmin = await _context.Roles
            .Where(x => userRoles.Contains(x.Id))
            .AnyAsync(x => x.Name == "Admin");

        var dismissedIds = await _context.DismissedNotifications
             .Where(x => x.UserId == userId)
             .Select(x => x.NotificationId)
             .ToListAsync();


        var queryResult = _context.Notifications.Where(x => !dismissedIds.Contains(x.Id));

        if (!isAdmin)
        {
            queryResult = queryResult.Where(x => x.TargetGroupId == 1);
        }
        else
        {
            queryResult = queryResult.Where(x => x.TargetGroupId == 2);
        }

        var notifications = await queryResult
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


    public async Task<List<string>> GetAdminUserIdsAsync()
    {
        var adminRole = await _context.Roles.FirstOrDefaultAsync(role => role.Name == "Admin");
        if (adminRole == null) return [];


        return await _context.UserRoles
            .Where(ur => ur.RoleId == adminRole.Id)
            .Select(ur => ur.UserId)
            .ToListAsync();
    }
}
