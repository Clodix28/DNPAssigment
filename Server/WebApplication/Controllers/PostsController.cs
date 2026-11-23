using ApiContracts.Comments;
using ApiContracts.Posts;
using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostsController(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            await _userRepository.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"User with ID '{request.UserId}' does not exist.");
        }

        Post post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
            SubForumId = request.SubForumId
        };

        Post created = await _postRepository.AddAsync(post);
        // send a dto back, the entity has User and Comments on it and would loop forever as json
        PostDto dto = ToDto(created);
        return Created($"/posts/{dto.Id}", dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(
        [FromQuery] string? titleContains = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null)
    {
        IQueryable<Post> query = _postRepository.GetMany()
            .Where(p => titleContains == null ||
                        p.Title.ToLower().Contains(titleContains.ToLower()))
            .Where(p => userId == null || p.UserId == userId);

        if (userName != null)
        {
            // thanks to the navigation property we can filter on the author's name right in the query
            query = query.Where(p => p.User.Username.ToLower().Contains(userName.ToLower()));
        }

        List<PostDto> posts = await query
            .Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                UserId = p.UserId,
                SubForumId = p.SubForumId
            })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(
        [FromRoute] int id,
        [FromQuery] bool includeAuthor = false,
        [FromQuery] bool includeComments = false)
    {
        IQueryable<Post> query = _postRepository.GetMany().Where(p => p.Id == id);

        if (includeAuthor)
        {
            query = query.Include(p => p.User);
        }

        if (includeComments)
        {
            query = query.Include(p => p.Comments);
        }

        // nothing has hit the database yet, FirstOrDefaultAsync turns all of it into one sql query
        PostDto? dto = await query
            .Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                UserId = p.UserId,
                SubForumId = p.SubForumId,
                Author = includeAuthor
                    ? new UserDto
                    {
                        Id = p.User.Id,
                        Username = p.User.Username,
                        Email = p.User.Email
                    }
                    : null,
                Comments = includeComments
                    ? p.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Body = c.Body,
                        UserId = c.UserId,
                        PostId = c.PostId
                    }).ToList()
                    : new()
            })
            .FirstOrDefaultAsync();

        if (dto == null)
        {
            return NotFound($"Post with ID '{id}' not found");
        }

        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            await _userRepository.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"User with ID '{request.UserId}' does not exist.");
        }

        try
        {
            Post post = new Post
            {
                Id = id,
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId,
                SubForumId = request.SubForumId
            };
            await _postRepository.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await _postRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private static PostDto ToDto(Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            SubForumId = post.SubForumId
        };
    }
}
