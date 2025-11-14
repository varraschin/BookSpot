using System.Security.Claims;
using BookSpot.UI.Services.Interfaces;

namespace BookSpot.UI.Services.Implementations;

public class UserContextService
{
    private readonly IAuthService _authService;

    public UserContextService(IAuthService authService)
    {
        _authService = authService;
    }

    public ClaimsPrincipal CreateClaimsPrincipal()
    {
        if (!_authService.IsAuthenticated())
            return new ClaimsPrincipal(new ClaimsIdentity());

        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, _authService.GetUserName()),
                new(ClaimTypes.Email, _authService.GetUserEmail()),
                new("Perfil", _authService.GetUserPerfil()),
                new(ClaimTypes.Role, _authService.GetUserPerfil())
            };

        var identity = new ClaimsIdentity(claims, "SessionAuth");
        return new ClaimsPrincipal(identity);
    }
}