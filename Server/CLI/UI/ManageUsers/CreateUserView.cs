using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView_
{
    private readonly IUserRepository  _userRepository;

    public CreateUserView_(IUserRepository userRepository)
    {
         _userRepository = userRepository;
    }
}