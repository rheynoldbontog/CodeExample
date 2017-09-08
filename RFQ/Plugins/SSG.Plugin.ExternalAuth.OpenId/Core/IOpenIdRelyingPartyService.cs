//Contributor:  Nicholas Mayne


using DotNetOpenAuth.OpenId.RelyingParty;

namespace SSG.Plugin.ExternalAuth.OpenId.Core
{
    public interface IOpenIdRelyingPartyService
    {
        IAuthenticationResponse Response { get; }
        IAuthenticationRequest CreateRequest(OpenIdIdentifier openIdIdentifier);
        bool HasResponse { get; }
    }
}