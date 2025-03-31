using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectUsersEntity
{
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }
    public virtual ProjectsEntity Project { get; set; } = null!;


    public string UserId { get; set; } = null!;
    public virtual UsersProfileEntity UserProfile { get; set; } = null!;

}
