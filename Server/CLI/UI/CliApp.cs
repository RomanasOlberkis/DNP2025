using System;
using System.Threading.Tasks;
using RepositoryContracts;
using Entities;
using CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;

    public CliApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }

    public async Task<Task> RunAsync()
    {
        Console.WriteLine("Running");
        bool running = true;

        while (running)
        {
            Console.WriteLine("......");
            Console.WriteLine("1 view post");
            Console.WriteLine("2 comment");
            Console.WriteLine("3 Exit");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var viewPostsView = new ViewPostsView(_postRepository);
                    await viewPostsView.ViewPostsAsync();
                    break;
                case "2":
                    Console.WriteLine("comment");
                    break;
                case "3":
                    running = false;
                    Console.WriteLine("Exit");
                    break;
                default:
                    Console.WriteLine("Invalid");
                    break;
            }
        }

        return Task.CompletedTask;
    }
}