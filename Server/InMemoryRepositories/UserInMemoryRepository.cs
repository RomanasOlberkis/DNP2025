using System;
using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new List<User>();

    public UserInMemoryRepository()
    {
        users.Add(new User { UserId = 1, Username = "Chungus", Password = "password" });
        users.Add(new User { UserId = 2, Username = "bingus", Password = "StrongPassword" });
    }
    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Any()
            ? users.Max(p => p.UserId) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task DeleteAsync(User user)
    {
        var existing = users.FirstOrDefault(p => p.UserId == user.UserId);
        if (existing != null)
        {
            users.Remove(existing);
        }
        return Task.CompletedTask;
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
    public Task UpdateAsync(User user)
    {
        var existing = users.FirstOrDefault(p => p.UserId == user.UserId);
        if (existing != null)
        {
            existing.Username = user.Username;
            existing.Password = user.Password;
        }
        return Task.CompletedTask;
    }
    public Task<User> GetSingleAsync(int UserId)
    {
        var user = users.FirstOrDefault(p => p.UserId == UserId);
        return Task.FromResult(user);
    }
}
