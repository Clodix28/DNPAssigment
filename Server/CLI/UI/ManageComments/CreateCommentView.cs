using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository  _commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        this._commentRepository = commentRepository;
    }
}