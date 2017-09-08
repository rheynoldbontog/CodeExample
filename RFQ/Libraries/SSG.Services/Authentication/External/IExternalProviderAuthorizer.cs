//Contributor:  Nicholas Mayne


namespace SSG.Services.Authentication.External
{
    public partial interface IExternalProviderAuthorizer
    {
        AuthorizeState Authorize(string returnUrl);
    }
}