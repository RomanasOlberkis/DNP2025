using System;
using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class PostInMemoryRepository: IPostRepository
{
     private List<Post> posts = new List<Post>();

    public PostInMemoryRepository()
    {
        var post1 = (new Post { PostId = 1, Title = "Help needed", Body = "All these squares make a circle" });
        post1.Comments.Add(new Comment { CommentId = 1, Body = "Big L" });
        var post2 = (new Post { PostId = 2, Title = "Am I cooked?", Body = "fortnite, big bucks, Big L small W" });
        post2.Comments.Add(new Comment { CommentId = 1, Body = "W Moment" });

        posts.Add(post1);
        posts.Add(post2);
    }

        public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Any()
            ? posts.Max(p => p.PostId) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

        public Task DeleteAsync(Post post)
        {
            var existing = posts.FirstOrDefault(p => p.PostId == post.PostId);
            if (existing != null)
            {
                posts.Remove(existing);
            }
            return Task.CompletedTask;
        }

        public IQueryable<Post> GetManyAsync()
        {
            return posts.AsQueryable();
        }
        public Task<Post?> UpdateAsync(Post post)
        {
            var existing = posts.FirstOrDefault(p => p.PostId == post.PostId);
            if (existing != null)
            {
                existing.Title = post.Title;
                existing.Body = post.Body;
            }
            return Task.FromResult(existing);
        }
        public Task<Post?> GetSingleAsync(int PostId)
        {
            var post = posts.FirstOrDefault(p => p.PostId == PostId);
            return Task.FromResult(post);
        }

    Task IPostRepository.UpdateAsync(Post post)
    {
        return UpdateAsync(post);
    }
}