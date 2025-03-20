using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class NotificationsRepository(DataContext context) : BaseRepository<NotificationEntity>(context), INotificationsRepository
{
}
