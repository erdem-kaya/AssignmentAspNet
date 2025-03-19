﻿namespace Data.Entities;

public class ProjectsEntity
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Budget { get; set; } = null!;
    public string? ProjectImage { get; set; }

    public int ClientId { get; set; }
    public virtual ClientsEntity Client { get; set; } = null!;

    public int UserId { get; set; }
    public virtual ApplicationUserEntity User { get; set; } = null!;

    public int ProjectStatusId { get; set; }
    public virtual ProjectStatusEntity ProjectStatus { get; set; } = null!;

    public virtual ICollection<NotificationEntity> Notifications { get; set; } = [];
    public virtual ICollection<ProjectUsersEntity> Projects { get; set; } = [];
}
