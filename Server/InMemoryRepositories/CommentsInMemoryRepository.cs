using System;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentsInMemoryRepository : ICommentRepository
{
     private List<Comment> comments = new List<Comment>();

        public Task<Comment> AddAsync(Comment comment)
        {
            comment.CommentId = comments.Any()
                ? comments.Max(p => p.CommentId) + 1
                : 1;
            comments.Add(comment);
            return Task.FromResult(comment);
        }

        public Task DeleteAsync(Comment comment)
        {
            var existing = comments.FirstOrDefault(p => p.CommentId == comment.CommentId);
            if (existing != null)
            {
                comments.Remove(existing);
            }
            return Task.CompletedTask;
        }

        public IQueryable<Comment> GetManyAsync()
        {
            return comments.AsQueryable();
        }
        public Task<Comment?> UpdateAsync(Comment comment)
        {
            var existing = comments.FirstOrDefault(p => p.CommentId == comment.CommentId);
            if (existing != null)
            {
                existing.Body = comment.Body;
            }
            return Task.FromResult(existing);
        }
        public Task<Comment?> GetSingleAsync(int CommentId)
        {
            var comment = comments.FirstOrDefault(p => p.CommentId == CommentId);
            return Task.FromResult(comment);
        }

    Task ICommentRepository.UpdateAsync(Comment comment)
    {
        return UpdateAsync(comment);
    }
}