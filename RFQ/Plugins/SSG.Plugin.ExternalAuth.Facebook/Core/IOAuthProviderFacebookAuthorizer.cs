//Contributor:  Nicholas Mayne

using Facebook;
using SSG.Core.Domain.Users;
using SSG.Services.Authentication.External;

namespace SSG.Plugin.ExternalAuth.Facebook.Core
{
    public interface IOAuthProviderFacebookAuthorizer : IExternalProviderAuthorizer
    {
        FacebookClient GetClient(User user);
    }
}