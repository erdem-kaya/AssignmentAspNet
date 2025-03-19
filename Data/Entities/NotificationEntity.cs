namespace Data.Entities;

public class NotificationEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; }
    public virtual ApplicationUserEntity? User { get; set; }

    public int? ProjectId { get; set; }
    public virtual ProjectsEntity? Project { get; set; }
}   
