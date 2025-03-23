namespace AspNetWebApiWithDbContext.Domain;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public Role Role { get; set; } = new Role();
    public User User { get; set; } = new User();
}
