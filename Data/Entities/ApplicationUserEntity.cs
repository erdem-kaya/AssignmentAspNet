using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class ApplicationUserEntity : IdentityUser
{
    public virtual UsersProfileEntity UsersProfile { get; set; } = null!;
}
