using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<IEnumerable<CommentDto>> GetCommentsAsync(int? userId = null, string? username = null, int? postId = null);
    Task<CommentDto> GetCommentAsync(int id);
    Task UpdateCommentAsync(int id, UpdateCommentDto request);
    Task DeleteCommentAsync(int id);
}