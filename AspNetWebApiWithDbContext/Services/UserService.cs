﻿using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApiWithDbContext.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserRole> userRoleRepo;
    private readonly IRepository<User> userRepo;

    public UserService(IRepository<UserRole> userRoleRepo,IRepository<User> userRepo)
    {
        
        this.userRoleRepo = userRoleRepo;
        this.userRepo = userRepo;
    }

    public async Task<List<UserListDto>> GetUsers(int count, int page)
    {
        var users = userRepo.GetPagedDataAsync(c => true,
            c => new UserListDto
            {
                Age = c.Age,
                Email = c.Email,
                FirstName = c.FirstName,
                Id = c.Id,
                LastName = c.LastName,
            }, c => c.OrderBy(d => d.LastName),page,count);

        return await users.ToListAsync();
    }

    public async Task<List<UserListDto>> GetUsersInRole(string RoleName)
    {
        var users = userRoleRepo.GetDataAsync(c => c.Role.Name == RoleName,
            c => new UserListDto
            {
                RoleName = c.Role.Name,
                Age = c.User.Age,
                Email = c.User.Email,
                FirstName = c.User.FirstName,
                Id = c.User.Id,
                LastName = c.User.LastName,
            }, c => c.OrderBy(d => d.User.LastName), c => c.User, c => c.Role);

        return await users.ToListAsync();

    }
}
