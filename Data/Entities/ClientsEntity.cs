namespace Data.Entities;

public class ClientsEntity
{
    public int Id { get; set; }
    public string ClientName { get; set; } = null!;
    public string? Email { get; set; }

    //Client kan ha flera projekt
    public ICollection<ProjectsEntity> Projects { get; set; } = [];
}
