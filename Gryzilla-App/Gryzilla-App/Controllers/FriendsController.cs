using Gryzilla_App.Repositories.Interfaces;

namespace Gryzilla_App.Controllers;

public class FriendsController
{
    private readonly IFriendsDbRepository _userDbRepository;
    public FriendsController(IFriendsDbRepository userDbRepository)
    {
        _userDbRepository = userDbRepository;
    }
}