using System;
using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User> GetSingleAsync(int UserId);
    IQueryable<User> GetManyAsync();
}
