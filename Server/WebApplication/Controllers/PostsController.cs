using ApiContracts.Posts;
using Entities;
using Microsoft.AspNetCore.Mvc;
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
        PostDto dto = ToDto(created);
        return Created($"/posts/{dto.Id}", dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery] string? titleContains = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null)
    {
        IEnumerable<Post> posts = _postRepository.GetMany()
            .Where(p => titleContains == null ||
                        p.Title.ToLower().Contains(titleContains.ToLower()))
            .Where(p => userId == null || p.UserId == userId)
            .ToList();

        if (userName != null)
        {
            HashSet<int> matchingUserIds = _userRepository.GetMany()
                .Where(u => u.Username.ToLower().Contains(userName.ToLower()))
                .Select(u => u.Id)
                .ToHashSet();
            posts = posts.Where(p => matchingUserIds.Contains(p.UserId));
        }

        return Ok(posts.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            Post post = await _postRepository.GetSingleAsync(id);
            return Ok(ToDto(post));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
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
