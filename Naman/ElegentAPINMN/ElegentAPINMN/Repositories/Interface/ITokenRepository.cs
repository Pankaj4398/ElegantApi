using Microsoft.AspNetCore.Identity;

namespace ElegentAPINMN.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
