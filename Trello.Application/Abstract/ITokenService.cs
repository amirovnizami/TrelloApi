using Trello.Domain.Entities;

namespace Trello.Application.Abstract;

public interface ITokenService
{
    string GenerateJWT(User user);
}