using AspNetWebApiWithDbContext.Domain;

namespace AspNetWebApiWithDbContext.Dtos;

public class UserUpdateDto:IBaseDto<User,UserUpdateDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public UserUpdateDto GetDto(User entity)
    {
        return new UserUpdateDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
        };
    }

    public User GetEntity()
    {
        return new User
        {
            FirstName = FirstName,
            LastName = LastName,
        };
    }
}
