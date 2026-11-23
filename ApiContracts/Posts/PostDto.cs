using ApiContracts.Comments;
using ApiContracts.Users;

namespace ApiContracts.Posts;

public class PostDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public int UserId { get; set; }
    public int SubForumId { get; set; }

    // only filled in when you ask for them, e.g. GET /posts/5?includeAuthor=true&includeComments=true
    public UserDto? Author { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}
