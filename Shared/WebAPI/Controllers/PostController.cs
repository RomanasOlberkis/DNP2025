using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

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
        try
        {
            await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"User with ID {request.UserId} not found");
        }

        Post post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };
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

        var postList = posts.ToList();
        var postDtos = new List<PostDto>();

        foreach (var post in postList)
        {
            try
            {
                User user = await userRepo.GetSingleAsync(post.UserId);
                
                if (!string.IsNullOrEmpty(username) && 
                    !user.UserName.Contains(username, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                postDtos.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    UserId = post.UserId,
                    UserName = user.UserName
                });
            }
            catch
            {
                postDtos.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    UserId = post.UserId,
                    UserName = "Unknown"
                });
            }
        }

        return Ok(postDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id, [FromQuery] bool includeComments = false)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            User user = await userRepo.GetSingleAsync(post.UserId);

            PostDto dto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                UserName = user.UserName
            };

            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
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
        try
        {
            await postRepo.GetSingleAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found");
        }

        var comments = commentRepo.GetMany().Where(c => c.PostId == id).ToList();
        var commentDtos = new List<CommentDto>();

        foreach (var comment in comments)
        {
            try
            {
                User user = await userRepo.GetSingleAsync(comment.UserId);
                commentDtos.Add(new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    UserId = comment.UserId,
                    UserName = user.UserName,
                    PostId = comment.PostId
                });
            }
            catch
            {
                commentDtos.Add(new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    UserId = comment.UserId,
                    UserName = "Unknown",
                    PostId = comment.PostId
                });
            }
        }

        return Ok(commentDtos);
    }
}