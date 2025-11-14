using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class ManageCommentsView
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public ManageCommentsView(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("write 1 to create a comment on a post");
            Console.WriteLine("write 2 to view all comments on a post");
            Console.WriteLine("write 3 to update specific post");
            Console.WriteLine("write 4 to delete a specific comment");
            Console.WriteLine("write 0 to go back to function selection");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await AddCommentAsync();
                    break;
                case "2":
                    await ListCommentsAsync();
                    break;
                case "3":
                    await UpdateCommentAsync();
                    break;
                case "4":
                    await DeleteCommentAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private async Task AddCommentAsync()
    {
        Console.Write("Post ID: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID");
            return;
        }

        Console.Write("User ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID");
            return;
        }

        Console.Write("Comment: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrEmpty(body))
        {
            Console.WriteLine("Comment cannot be empty");
            return;
        }

        Comment comment = new Comment { Body = body, UserId = userId, PostId = postId };
        Comment created = await commentRepo.AddAsync(comment);
        Console.WriteLine($"Comment created with ID: {created.Id}");
    }

    private async Task ListCommentsAsync()
    {
        Console.Write("Filter by Post ID (leave empty for all): ");
        string? input = Console.ReadLine();
        
        var comments = commentRepo.GetMany();
        
        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int postId))
        {
            comments = comments.Where(c => c.PostId == postId);
        }

        var commentList = comments.ToList();
        Console.WriteLine("\n=== Comments ===");
        foreach (var comment in commentList)
        {
            try
            {
                User user = await userRepo.GetSingleAsync(comment.UserId);
                Console.WriteLine($"[{comment.Id}] {user.UserName}: {comment.Body}");
            }
            catch
            {
                Console.WriteLine($"[{comment.Id}] Unknown user: {comment.Body}");
            }
        }
    }

    private async Task UpdateCommentAsync()
    {
        Console.Write("Comment ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            Console.Write($"New comment (current: {comment.Body}): ");
            string? body = Console.ReadLine();

            if (!string.IsNullOrEmpty(body)) comment.Body = body;

            await commentRepo.UpdateAsync(comment);
            Console.WriteLine("Comment updated");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private async Task DeleteCommentAsync()
    {
        Console.Write("Comment ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        try
        {
            await commentRepo.DeleteAsync(id);
            Console.WriteLine("Comment deleted");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}