using System;
using RepositoryContracts;
using Entities;

namespace CLI.UI;

public class ViewPostsView
{
    public readonly IPostRepository _postRepository;

    public ViewPostsView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    private async Task ViewCommentsAsync(Post selectedPost)
{
    if (selectedPost.Comments == null || !selectedPost.Comments.Any())
    {
        Console.WriteLine("No comments");
        return;
    }

    Console.WriteLine("Comments:");
    foreach (var comment in selectedPost.Comments)
    {
        Console.WriteLine($"- {comment.Body} (ID: {comment.CommentId})");
    }
}

    public async Task ViewPostsAsync()
    {
        var posts = _postRepository.GetManyAsync().ToList();

        if (posts == null || !posts.Any())
        {
            Console.WriteLine("No posts available.");
            return;
        }

        Console.WriteLine("Posts:");
        for (int i = 0; i < posts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {posts[i].Title}");
        }

        Console.Write("Enter the number of the post to view details: ");
        string? choice = Console.ReadLine();

        if (int.TryParse(choice, out int postIndex) && postIndex > 0 && postIndex <= posts.Count)
        {
            var selectedPost = posts[postIndex - 1]; 
            Console.WriteLine($"\nTitle: {selectedPost.Title}");
            Console.WriteLine($"Content: {selectedPost.Body}");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }

        if (int.TryParse(choice, out _) && postIndex > 0 && postIndex <= posts.Count)
        {
            var selectedPost = posts[postIndex - 1];
            Console.WriteLine($"\nTitle: {selectedPost.Title}");
            Console.WriteLine($"Body: {selectedPost.Body}");
            await ViewCommentsAsync(selectedPost);
        }

    }
}
