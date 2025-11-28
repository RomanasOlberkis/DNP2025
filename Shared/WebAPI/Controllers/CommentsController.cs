using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo, IPostRepository postRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"User with ID {request.UserId} not found");
        }

        try
        {
            await postRepo.GetSingleAsync(request.PostId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"Post with ID {request.PostId} not found");
        }

    Comment comment = new Comment(request.Body, request.UserId, request.PostId);
        Comment created = await commentRepo.AddAsync(comment);
        User user = await userRepo.GetSingleAsync(created.UserId);

        CommentDto dto = new CommentDto
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            UserName = user.UserName,
            PostId = created.PostId
        };
        return Created($"/comments/{dto.Id}", dto);
    }

[HttpGet]
public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(
    [FromQuery] int? userId,
    [FromQuery] string? username,
    [FromQuery] int? postId)
{
    IQueryable<Comment> comments = commentRepo.GetMany().Include(c => c.User);

    if (userId.HasValue)
    {
        comments = comments.Where(c => c.UserId == userId.Value);
    }

    if (postId.HasValue)
    {
        comments = comments.Where(c => c.PostId == postId.Value);
    }

    if (!string.IsNullOrEmpty(username))
    {
        comments = comments.Where(c => c.User.UserName.Contains(username, StringComparison.OrdinalIgnoreCase));
    }

    var commentDtos = await comments.Select(c => new CommentDto
    {
        Id = c.Id,
        Body = c.Body,
        UserId = c.UserId,
        UserName = c.User.UserName,
        PostId = c.PostId
    }).ToListAsync();

    return Ok(commentDtos);
}

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            User user = await userRepo.GetSingleAsync(comment.UserId);

            CommentDto dto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                UserName = user.UserName,
                PostId = comment.PostId
            };

            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            comment.Body = request.Body;
            await commentRepo.UpdateAsync(comment);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
}