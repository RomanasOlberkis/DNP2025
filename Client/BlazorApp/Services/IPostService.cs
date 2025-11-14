using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<IEnumerable<PostDto>> GetPostsAsync(string? title = null, int? userId = null, string? username = null);
    Task<PostDto> GetPostAsync(int id);
    Task UpdatePostAsync(int id, UpdatePostDto request);
    Task DeletePostAsync(int id);
    Task<IEnumerable<CommentDto>> GetPostCommentsAsync(int postId);
}