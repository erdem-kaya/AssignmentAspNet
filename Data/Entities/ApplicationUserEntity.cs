using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class ApplicationUserEntity : IdentityUser
{
    //User profil handlar om att skapa en relation mellan ApplicationUserEntity och UsersProfileEntity
    public virtual UsersProfileEntity? UsersProfile { get; set; }

    public virtual ICollection<NotificationDismissedEntity> DismissedNotifications { get; set; } = [];
}
