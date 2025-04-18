﻿using AspNetWebApiWithDbContext.Domain;

namespace AspNetWebApiWithDbContext.Dtos;

public class UserListDto:IBaseDto<User,UserListDto>
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public UserListDto GetDto(User entity)
    {
        return new UserListDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Username = entity.Username,
        };
    }

    public User GetEntity()
    {
        return new User
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Username = Username,
        };
    }
}
