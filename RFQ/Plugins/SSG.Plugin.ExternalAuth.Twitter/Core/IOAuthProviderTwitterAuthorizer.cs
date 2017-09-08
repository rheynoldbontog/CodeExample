//Contributor:  Nicholas Mayne

using LinqToTwitter;
using SSG.Core.Domain.Users;
using SSG.Services.Authentication.External;

namespace SSG.Plugin.ExternalAuth.Twitter.Core
{
    public interface IOAuthProviderTwitterAuthorizer : IExternalProviderAuthorizer
    {
        ITwitterAuthorizer GetAuthorizer(SSG.Core.Domain.Users.User user);
    }
}