using Newtonsoft.Json;
using RepositoryContracts;
using Entities;

public class UserFileRepository : IUserRepository
{
    private readonly string _filePath;

    public UserFileRepository(string filePath)
    {
        _filePath = filePath;
    }

    private async Task<List<User>> ReadFromFileAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<User>(); // Return an empty list if the file doesn't exist
        }

        try
        {
            string jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(jsonString) ?? new List<User>();
        }
        catch (JsonException)
        {
            // Handle JSON parsing errors (e.g., corrupted file)
            return new List<User>(); // Or throw an exception, log the error, etc.
        }
    }

    private async Task WriteToFileAsync(List<User> users)
    {
        string jsonString = JsonConvert.SerializeObject(users, Formatting.Indented);
        await File.WriteAllTextAsync(_filePath, jsonString);
    }

    public async Task<User?> GetSingleAsync(int id)
    {
        var users = await ReadFromFileAsync();
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<List<User>> GetManyAsync()
    {
        return await ReadFromFileAsync();
    }

    public async Task AddAsync(User user)
    {
        var users = await ReadFromFileAsync();
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1; // Simple ID generation
        users.Add(user);
        await WriteToFileAsync(users);
    }

    public async Task UpdateAsync(User user)
    {
        var users = await ReadFromFileAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            await WriteToFileAsync(users);
        }
    }

    public async Task DeleteAsync(User user)
    {
        var users = await ReadFromFileAsync();
        users.RemoveAll(u => u.Id == user.Id);
        await WriteToFileAsync(users);
    }
}