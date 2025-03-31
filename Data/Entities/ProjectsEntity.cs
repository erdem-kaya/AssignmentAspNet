using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectsEntity
{
    [Key]
    public int Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Budget { get; set; }
    public string? ProjectImage { get; set; }

    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public virtual ClientsEntity Client { get; set; } = null!;

    [ForeignKey(nameof(ProjectStatus))]
    public int ProjectStatusId { get; set; }
    public virtual ProjectStatusEntity ProjectStatus { get; set; } = null!;

    public virtual ICollection<ProjectUsersEntity> ProjectWithUsers { get; set; } = [];
}
