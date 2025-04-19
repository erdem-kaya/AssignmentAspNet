using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUserEntity>(options)
{
    public DbSet<ClientsEntity> Clients { get; set; }
    public DbSet<ProjectsEntity> Projects { get; set; }
    public DbSet<ProjectStatusEntity> ProjectStatus { get; set; }
    public DbSet<ProjectUsersEntity> ProjectUsers { get; set; }
    public DbSet<UsersProfileEntity> UsersProfile { get; set; }
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<NotificationDismissedEntity> DismissedNotifications { get; set; }
    public DbSet<NotificationTypeEntity> NotificationTypes { get; set; }
    public DbSet<NotificationTargetGroupEntity> NotificationTargetGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUserEntity>()
            .HasOne(a => a.UsersProfile)
            .WithOne(up => up.ApplicationUser)
            .HasForeignKey<UsersProfileEntity>(up => up.Id)
            .HasPrincipalKey<ApplicationUserEntity>(a => a.Id);

        modelBuilder.Entity<ProjectUsersEntity>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectWithUsers)
            .HasForeignKey(pu => pu.ProjectId);

        modelBuilder.Entity<ProjectUsersEntity>()
            .HasOne(pu => pu.UserProfile)
            .WithMany()
            .HasForeignKey(pu => pu.UserId);
    }
}
