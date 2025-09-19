using InMemoryRepositories;
using RepositoryContracts;
using CLI.UI;
using System;
using Entities;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("CLI App is starting...");

        IUserRepository userRepository = new UserInMemoryRepository();
        IPostRepository postRepository = new PostInMemoryRepository();
        ICommentRepository commentRepository = new CommentsInMemoryRepository();

        CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);

        try
        {
            await cliApp.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}