using System.Security.Claims;
using Trello.Application.Security;

namespace Trello.WebUi.Infrastructure;

public class HttpUserContext : IUserContext
{
    private readonly int? _userId;

    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            _userId = null;
            return;
        }

        var idClaim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(idClaim, out var userId))
        {
            _userId = null;
        }
        else
        {
            _userId = userId;
        }
    }

    public int? UserId => _userId;

    public int MustGetUserId()
    {
        if (_userId is null)
        {
            throw new InvalidOperationException("User ID is missing or invalid.");
        }

        return _userId.Value;
    }
}