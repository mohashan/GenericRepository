﻿namespace AspNetWebApiWithDbContext.Domain;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
