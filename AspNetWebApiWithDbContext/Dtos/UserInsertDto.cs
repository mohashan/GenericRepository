using AspNetWebApiWithDbContext.Domain;

namespace AspNetWebApiWithDbContext.Dtos;

public class UserInsertDto:IBaseDto<User, UserInsertDto>
{

    public string Username{ get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }

    public User GetEntity()
    {
        return new User
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Age = Age,
            Username = Username,
            Password = Password,
        };
    }

    public UserInsertDto GetDto(User entity)
    {
        return new UserInsertDto
        {
            Username = entity.Username,
            Age = entity.Age,
            Password = entity.Password,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
        };
    }
}
