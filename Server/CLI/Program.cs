using FileRepositories;
using RepositoryContracts;
using CLI.UI;

IUserRepository userRepo = new UserFileRepository();
IPostRepository postRepo = new PostFileRepository();
ICommentRepository commentRepo = new CommentFileRepository();

CliApp app = new CliApp(userRepo, postRepo, commentRepo);
await app.StartAsync();