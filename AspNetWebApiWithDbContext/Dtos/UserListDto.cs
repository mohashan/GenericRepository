﻿namespace AspNetWebApiWithDbContext.Dtos;

public class UserListDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public int Age { get; set; }
}
