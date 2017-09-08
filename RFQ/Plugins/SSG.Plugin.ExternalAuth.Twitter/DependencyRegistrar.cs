using Autofac;
using Autofac.Integration.Mvc;
using SSG.Core.Infrastructure;
using SSG.Core.Infrastructure.DependencyManagement;
using SSG.Plugin.ExternalAuth.Twitter.Core;

namespace SSG.Plugin.ExternalAuth.Twitter
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<TwitterProviderAuthorizer>().As<IOAuthProviderTwitterAuthorizer>().InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}