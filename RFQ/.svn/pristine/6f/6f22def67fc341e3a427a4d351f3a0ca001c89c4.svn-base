using Autofac;
using Autofac.Integration.Mvc;
using SSG.Core.Infrastructure;
using SSG.Core.Infrastructure.DependencyManagement;
using SSG.Plugin.ExternalAuth.Facebook.Core;

namespace SSG.Plugin.ExternalAuth.Facebook
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<FacebookProviderAuthorizer>().As<IOAuthProviderFacebookAuthorizer>().InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
