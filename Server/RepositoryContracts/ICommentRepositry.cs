using System;
using Entities;

namespace RepositoryContracts;

public interface ICommentRepositry
{
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Comment comment);
    Task<Comment> GetSingleAsync(int CommentId);
    IQueryable<Comment> GetManyAsync();
}
