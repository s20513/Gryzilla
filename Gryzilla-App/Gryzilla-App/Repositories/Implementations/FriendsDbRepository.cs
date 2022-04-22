using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;

namespace Gryzilla_App.Repositories.Implementations;

public class FriendsDbRepository : IFriendsDbRepository
{
    private readonly GryzillaContext _context;
    public FriendsDbRepository(GryzillaContext context)
    {
        _context = context;
    }

}