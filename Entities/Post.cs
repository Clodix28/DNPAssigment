namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    // EF picks UserId up as the foreign key for User just from the name
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SubForumId { get; set; }

    public List<Comment> Comments { get; set; } = new();
}
