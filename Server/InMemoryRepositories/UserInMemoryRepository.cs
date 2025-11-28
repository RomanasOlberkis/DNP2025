using System;
using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new List<User>();

     public UserInMemoryRepository()
    {
        users.Add(CreateUser(1, "Chungus", "password"));
        users.Add(CreateUser(2, "bingus", "StrongPassword"));
    }

    private static User CreateUser(int id, string userName, string password)
    {
        var userObj = (User)Activator.CreateInstance(typeof(User), nonPublic: true) ?? throw new InvalidOperationException("Could not create User instance");
        var type = typeof(User);
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

        var idProp = type.GetProperty("Id", flags);
        var userNameProp = type.GetProperty("UserName", flags);
        var passwordProp = type.GetProperty("Password", flags);

        if (idProp != null && idProp.CanWrite)
            idProp.SetValue(userObj, id);
        else if (idProp != null)
            type.GetField("<Id>k__BackingField", flags)?.SetValue(userObj, id);

        if (userNameProp != null && userNameProp.CanWrite)
            userNameProp.SetValue(userObj, userName);
        else if (userNameProp != null)
            type.GetField("<UserName>k__BackingField", flags)?.SetValue(userObj, userName);

        if (passwordProp != null && passwordProp.CanWrite)
            passwordProp.SetValue(userObj, password);
        else if (passwordProp != null)
            type.GetField("<Password>k__BackingField", flags)?.SetValue(userObj, password);

        return userObj;
    }
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        return Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}
