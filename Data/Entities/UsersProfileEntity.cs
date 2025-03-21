using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UsersProfileEntity
{
    //Id är samma som Id i ApplicationUser
    [Key]
    [ForeignKey(nameof(ApplicationUser))]
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime Birthday { get; set; }
    public string? ProfilePicture { get; set; }


    public virtual ApplicationUserEntity ApplicationUser { get; set; } = null!;

    public int JobTitleId { get; set; }
    public virtual JobTitlesEntity JobTitle { get; set; } = null!;
}