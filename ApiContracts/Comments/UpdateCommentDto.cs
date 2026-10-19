namespace ApiContracts.Comments;

public class UpdateCommentDto
{
    public required string Body { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
}
