using ApiContracts.Comments;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public CommentsController(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
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
            await _postRepository.GetSingleAsync(request.PostId);
        }
        catch (InvalidOperationException)
        {
            return BadRequest($"Post with ID '{request.PostId}' does not exist.");
        }

        Comment comment = new Comment
        {
            Body = request.Body,
            UserId = request.UserId,
            PostId = request.PostId
        };

        Comment created = await _commentRepository.AddAsync(comment);
        CommentDto dto = ToDto(created);
        return Created($"/comments/{dto.Id}", dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null,
        [FromQuery] int? postId = null)
    {
        IEnumerable<Comment> comments = _commentRepository.GetMany()
            .Where(c => userId == null || c.UserId == userId)
            .Where(c => postId == null || c.PostId == postId)
            .ToList();

        if (userName != null)
        {
            HashSet<int> matchingUserIds = _userRepository.GetMany()
                .Where(u => u.Username.ToLower().Contains(userName.ToLower()))
                .Select(u => u.Id)
                .ToHashSet();
            comments = comments.Where(c => matchingUserIds.Contains(c.UserId));
        }

        return Ok(comments.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            Comment comment = await _commentRepository.GetSingleAsync(id);
            return Ok(ToDto(comment));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment comment = new Comment
            {
                Id = id,
                Body = request.Body,
                UserId = request.UserId,
                PostId = request.PostId
            };
            Comment updated = await _commentRepository.UpdateAsync(comment);
            return Ok(ToDto(updated));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await _commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private static CommentDto ToDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId
        };
    }
}
