using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository _commentRepository;
    
    public ListCommentsView (ICommentRepository commentRepository)
        {
        this._commentRepository = commentRepository;
        }
}