/*using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;

    public CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
    {
        this.userRepo = userRepo;
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
    }
    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine("write 1 to maanage users");
            Console.WriteLine("write 2 to make a post");
            Console.WriteLine(" write 3 for comments");
            Console.WriteLine("0 to exit the application");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ManageUsersAsync();
                    break;
                case "2":
                    await ManagePostsAsync();
                    break;
                case "3":
                    await ManageCommentsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private async Task ManageUsersAsync()
    {
        ManageUsersView view = new ManageUsersView(userRepo);
        await view.ShowAsync();
    }

    private async Task ManagePostsAsync()
    {
        ManagePostsView view = new ManagePostsView(postRepo, userRepo);
        await view.ShowAsync();
    }

    private async Task ManageCommentsAsync()
    {
        ManageCommentsView view = new ManageCommentsView(commentRepo, postRepo, userRepo);
        await view.ShowAsync();
    }
}*/