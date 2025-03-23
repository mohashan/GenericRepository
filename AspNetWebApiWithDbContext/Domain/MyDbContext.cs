using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace AspNetWebApiWithDbContext.Domain;

public class MyDbContext:DbContext
{
    public MyDbContext(DbContextOptions options):base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(c => c.Username)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(c => c.Password)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<UserRole>()
            .HasOne(c => c.Role)
            .WithMany(c=>c.UserRoles)
            .HasForeignKey(c=>c.RoleId)
            .IsRequired();

        modelBuilder.Entity<UserRole>()
            .HasIndex(c => new { c.RoleId, c.UserId })
            .IsUnique();

        modelBuilder.Entity<UserRole>()
            .HasOne(c => c.User)
            .WithMany(c => c.UserRoles)
            .HasForeignKey(c=>c.UserId)
            .IsRequired();

        modelBuilder.Entity<Role>()
            .Property(c=>c.Name)
            .IsRequired()
            .HasMaxLength(50);

    }
}
