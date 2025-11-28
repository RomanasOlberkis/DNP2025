using System;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> comments = new();
    public CommentInMemoryRepository()
    {
        // Create instances using reflection since Comment's constructor is non-public
        Comment c1 = (Comment)Activator.CreateInstance(typeof(Comment), true);
        c1.Id = 1;
        c1.Body = "Big L";
        c1.UserId = 2;
        c1.PostId = 1;
        comments.Add(c1);

        Comment c2 = (Comment)Activator.CreateInstance(typeof(Comment), true);
        c2.Id = 2;
        c2.Body = "W Moment";
        c2.UserId = 1;
        c2.PostId = 2;
        comments.Add(c2);
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with this ID: '{comment.Id}' not found in among the users on this very specific beutiful site");
        }
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with this ID: '{id}' not found in among the users on this very specific beutiful site");
        }
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with this ID: '{id}' not found in among the users on this very specific beutiful site");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}