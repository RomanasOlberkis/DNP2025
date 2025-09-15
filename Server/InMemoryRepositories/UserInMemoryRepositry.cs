using System;
using System.Formats.Tar;
using System.Reflection.Metadata;
using System.User;

namespace InMemoryRepositories;

public Task <User> AddAsync(User user)
{
    user.UserId = users.Any()
        ? users.Max(p => p.UserId) + 1
        : 1;
    users.Add(user);
    return Task.FromResult(user)
}
