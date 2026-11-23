namespace Entities;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // navigation properties for EF, the other side is Post.User and Comment.User
    public List<Post> Posts { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}
