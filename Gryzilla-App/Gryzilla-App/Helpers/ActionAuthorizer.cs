using System.Security.Claims;

namespace Gryzilla_App.Helpers;

public static class ActionAuthorizer
{
    public static bool IsAuthorOrHasRightRole(ClaimsPrincipal userClaims, int idCreator)
    {
        var idUser = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;

        return idUser == idCreator.ToString() || userRole == "Admin" || userRole == "Moderator";
    }
    
    public static bool IsAuthorOrAdmin(ClaimsPrincipal userClaims, int idCreator)
    {
        var idUser = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;

        return idUser == idCreator.ToString() || userRole == "Admin";
    }

    public static bool IsTheUserToRemoveFromGroupOrAuthorOrHasRightRole(ClaimsPrincipal userClaims, int idCreator, int idUserMember)
    {
        var idUser = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return IsAuthorOrHasRightRole(userClaims, idCreator) || idUser == idUserMember.ToString();
    }
    
    public static bool IsAuthorOrHasRightRoleOrIsProfileOrGroupOwner(ClaimsPrincipal userClaims, int idCreator, int idOwner)
    {
        var idUser = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;

        return idUser == idCreator.ToString() || idUser == idOwner.ToString()  || userRole == "Admin" || userRole == "Moderator";
    }
}