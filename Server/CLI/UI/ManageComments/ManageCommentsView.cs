using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView
{
    private readonly ICommentRepository _commentRepository;
    
    public ManageCommentsView (ICommentRepository commentRepository)
    {
        this._commentRepository = commentRepository;
    }
}