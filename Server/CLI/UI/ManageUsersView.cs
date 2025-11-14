using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class ManageUsersView
{
    private readonly IUserRepository userRepo;

    public ManageUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("write 1 to create a new user");
            Console.WriteLine("write 2 to see all the users");
            Console.WriteLine("write 3 to update specific user info");
            Console.WriteLine("write 4 to update a specific user");
            Console.WriteLine("write 0 to go back to function selection");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    await ListUsersAsync();
                    break;
                case "3":
                    await UpdateUserAsync();
                    break;
                case "4":
                    await DeleteUserAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        Console.Write("Password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Empty user name or password");
            return;
        }

        User user = new User { UserName = username, Password = password };
        User created = await userRepo.AddAsync(user);
        Console.WriteLine($"User created with ID: {created.Id}");
    }

    private async Task ListUsersAsync()
    {
        var users = userRepo.GetMany().ToList();
        Console.WriteLine("Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"[{user.Id}] {user.UserName}");
        }
    }

    private async Task UpdateUserAsync()
    {
        Console.Write("User ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid user ID");
            return;
        }

        try
        {
            User user = await userRepo.GetSingleAsync(id);
            Console.Write($"New username (current: {user.UserName}): ");
            string? username = Console.ReadLine();
            Console.Write("New password: ");
            string? password = Console.ReadLine();

            if (!string.IsNullOrEmpty(username)) user.UserName = username;
            if (!string.IsNullOrEmpty(password)) user.Password = password;

            await userRepo.UpdateAsync(user);
            Console.WriteLine("User updated");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private async Task DeleteUserAsync()
    {
        Console.Write("User ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            return;
        }

        try
        {
            await userRepo.DeleteAsync(id);
            Console.WriteLine("User deleted");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}