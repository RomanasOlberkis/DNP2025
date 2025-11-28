using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

   [HttpPost]
public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
{
    bool userExists = await userRepo.GetMany().AnyAsync(u => u.Id == request.UserId);
    if (!userExists)
    {
        return BadRequest($"User with ID {request.UserId} not found");
    }

    Post post = new Post(request.Title, request.Body, request.UserId);
    Post created = await postRepo.AddAsync(post);
    
    User user = await userRepo.GetSingleAsync(created.UserId);

    PostDto dto = new PostDto
    {
        Id = created.Id,
        Title = created.Title,
        Body = created.Body,
        UserId = created.UserId,
        UserName = user.UserName
    };
    return Created($"/posts/{dto.Id}", dto);
}

    [HttpGet]
public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(
    [FromQuery] string? title, 
    [FromQuery] int? userId,
    [FromQuery] string? username)
{
    var posts = postRepo.GetMany();

    if (!string.IsNullOrEmpty(title))
    {
        posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
    }

    if (userId.HasValue)
    {
        posts = posts.Where(p => p.UserId == userId.Value);
    }

    if (!string.IsNullOrEmpty(username))
    {
        posts = posts.Where(p => p.User.UserName.Contains(username, StringComparison.OrdinalIgnoreCase));
    }

    posts = posts.Include(p => p.User);

    var postDtos = await posts.Select(p => new PostDto
    {
        Id = p.Id,
        Title = p.Title,
        Body = p.Body,
        UserId = p.UserId,
        UserName = p.User.UserName
    }).ToListAsync();

    return Ok(postDtos);
}

    [HttpGet("{id}")]
public async Task<ActionResult<PostDto>> GetPost(int id, [FromQuery] bool includeComments = false)
{
    var query = postRepo.GetMany()
        .Where(p => p.Id == id)
        .Include(p => p.User);

    if (includeComments)
    {
        query = query.Include(p => p.Comments).ThenInclude(c => c.User);
    }

    var post = await query.FirstOrDefaultAsync();

    if (post == null)
    {
        return NotFound($"Post with ID {id} not found");
    }

    PostDto dto = new PostDto
    {
        Id = post.Id,
        Title = post.Title,
        Body = post.Body,
        UserId = post.UserId,
        UserName = post.User.UserName
    };

    return Ok(dto);
}

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            post.Title = request.Title;
            post.Body = request.Body;
            await postRepo.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id}/comments")]
public async Task<ActionResult<IEnumerable<CommentDto>>> GetPostComments(int id)
{
    bool postExists = await postRepo.GetMany().AnyAsync(p => p.Id == id);
    if (!postExists)
    {
        return NotFound($"Post with ID {id} not found");
    }

    var comments = await commentRepo.GetMany()
        .Where(c => c.PostId == id)
        .Include(c => c.User)
        .Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            UserName = c.User.UserName,
            PostId = c.PostId
        })
        .ToListAsync();

    return Ok(comments);
}
}