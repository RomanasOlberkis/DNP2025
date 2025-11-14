using FileRepositories;
using RepositoryContracts;
using CLI.UI;

IUserRepository userRepo = new UserFileRepository();
IPostRepository postRepo = new PostFileRepository();
ICommentRepository commentRepo = new CommentFileRepository();

CliApp app = new CliApp(userRepo, postRepo, commentRepo);
await app.StartAsync();
/*
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