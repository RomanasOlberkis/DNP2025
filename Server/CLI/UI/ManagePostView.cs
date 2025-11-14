using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class ManagePostsView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public ManagePostsView(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("write 1 to create a new post");
            Console.WriteLine("write 2 to see all the posts");
            Console.WriteLine("write 3 to update specific post info");
            Console.WriteLine("write 4 to update a specific user");
            Console.WriteLine("write 5 to delete a post");
            Console.WriteLine("write 0 to go back to function selection");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2":
                    await ListPostsAsync();
                    break;
                case "3":
                    await ViewPostAsync();
                    break;
                case "4":
                    await UpdatePostAsync();
                    break;
                case "5":
                    await DeletePostAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private async Task CreatePostAsync()
    {
        Console.Write("User ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID");
            return;
        }

        Console.Write("Title: ");
        string? title = Console.ReadLine();
        Console.Write("Body: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body))
        {
            Console.WriteLine("Title and body cannot be empty");
            return;
        }

        Post post = new Post { Title = title, Body = body, UserId = userId };
        Post created = await postRepo.AddAsync(post);
        Console.WriteLine($"Post created with ID: {created.Id}");
    }

    private async Task ListPostsAsync()
    {
        var posts = postRepo.GetMany().ToList();
        Console.WriteLine("\n=== Posts ===");
        foreach (var post in posts)
        {
            Console.WriteLine($"[{post.Id}] {post.Title}");
        }
    }

    private async Task ViewPostAsync()
    {
        Console.Write("Post ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            User user = await userRepo.GetSingleAsync(post.UserId);
            
            Console.WriteLine($"\nPost ID: #{post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Author: {user.UserName}");
            Console.WriteLine($"Body: {post.Body}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private async Task UpdatePostAsync()
    {
        Console.Write("Post ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            Console.Write($"New title (current: {post.Title}): ");
            string? title = Console.ReadLine();
            Console.Write($"New body (current: {post.Body}): ");
            string? body = Console.ReadLine();

            if (!string.IsNullOrEmpty(title)) post.Title = title;
            if (!string.IsNullOrEmpty(body)) post.Body = body;

            await postRepo.UpdateAsync(post);
            Console.WriteLine("Post updated");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private async Task DeletePostAsync()
    {
        Console.Write("Post ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        try
        {
            await postRepo.DeleteAsync(id);
            Console.WriteLine("Post has been deleted");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}