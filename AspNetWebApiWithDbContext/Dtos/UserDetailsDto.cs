using AspNetWebApiWithDbContext.Domain;

namespace AspNetWebApiWithDbContext.Dtos;
public class UserDetailsDto:IBaseDto<User, UserDetailsDto>
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> RoleNames { get; set; } = new List<string>();
    public int Age { get; set; }


    public User GetEntity()
    {
        return new User
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Age = Age,
            Username = Username,
        };
    }

    public UserDetailsDto GetDto(User entity)
    {
        return new UserDetailsDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Age = entity.Age,
            Username = entity.Username,
            RoleNames = [.. entity.UserRoles.Select(c => c.Role?.Name ?? string.Empty)],
        };
    }
}
