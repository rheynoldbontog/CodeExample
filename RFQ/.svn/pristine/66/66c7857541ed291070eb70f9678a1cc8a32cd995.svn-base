using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using SSG.Core.Data;
using SSG.Core.Infrastructure;
using SSG.Core.Infrastructure.DependencyManagement;
using SSG.Data;
using SSG.Plugin.Feed.Froogle.Data;
using SSG.Plugin.Feed.Froogle.Domain;
using SSG.Plugin.Feed.Froogle.Services;

namespace SSG.Plugin.Feed.Froogle
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<GoogleService>().As<IGoogleService>().InstancePerHttpRequest();

            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                //register named context
                builder.Register<IDbContext>(c => new GoogleProductObjectContext(dataProviderSettings.DataConnectionString))
                    .Named<IDbContext>("nop_object_context_google_product")
                    .InstancePerHttpRequest();

                builder.Register<GoogleProductObjectContext>(c => new GoogleProductObjectContext(dataProviderSettings.DataConnectionString))
                    .InstancePerHttpRequest();
            }
            else
            {
                //register named context
                builder.Register<IDbContext>(c => new GoogleProductObjectContext(c.Resolve<DataSettings>().DataConnectionString))
                    .Named<IDbContext>("nop_object_context_google_product")
                    .InstancePerHttpRequest();

                builder.Register<GoogleProductObjectContext>(c => new GoogleProductObjectContext(c.Resolve<DataSettings>().DataConnectionString))
                    .InstancePerHttpRequest();
            }

            //override required repository with our custom context
            builder.RegisterType<EfRepository<GoogleProductRecord>>()
                .As<IRepository<GoogleProductRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_google_product"))
                .InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
