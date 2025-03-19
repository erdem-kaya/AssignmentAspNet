namespace Data.Entities;

public class ProjectUsersEntity
{
    public int Id { get; set; }

    public int ProjectId { get; set; }
    public virtual ProjectsEntity Project { get; set; } = null!;


    public string? UserId { get; set; }
    public virtual ApplicationUserEntity User { get; set; } = null!;
}
