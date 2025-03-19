using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UsersProfileEntity
{
    [Key]
    [ForeignKey("ApplicationUserEntity")]
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime Brithday { get; set; }
    public string? ProfilePicture { get; set; }

    public int UserRoleId { get; set; }
    public virtual UserRolesEntity Role { get; set; } = null!;

    public int JobTitleId { get; set; }
    public virtual JobTitlesEntity JobTitle { get; set; } = null!;

    public virtual ICollection<NotificationEntity> Notifications { get; set; } = [];
    public virtual ICollection<ProjectsEntity> Projects { get; set; } = [];
}