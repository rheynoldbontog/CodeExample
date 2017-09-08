﻿using System;
using System.Globalization;
using System.Threading;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using SSG.Core;
using SSG.Core.Data;
using SSG.Core.Domain;
using SSG.Core.Infrastructure;
using SSG.Services.Logging;
using SSG.Services.Tasks;
using SSG.Web.Framework;
using SSG.Web.Framework.EmbeddedViews;
using SSG.Web.Framework.Mvc;
using SSG.Web.Framework.Mvc.Routes;
using SSG.Web.Framework.Themes;
using StackExchange.Profiling;
using StackExchange.Profiling.MVCHelpers;
using System.Net;
using SSG.Web.ActionFilter;
using SSG.Web.CustomModelBinders;

namespace SSG.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //do not register HandleErrorAttribute. use classic error handling mode
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);
            
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "SSG.Web.Controllers" }
            );
        }

        protected void Application_Start()
        {
            //initialize engine context
            EngineContext.Initialize(false);

            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();

            //set dependency resolver
            var dependencyResolver = new SSGDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);

            //model binders
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(BaseSSGModel), new SSGModelBinder());

            if (databaseInstalled)
            {
                //remove all view engines
                ViewEngines.Engines.Clear();
                //except the themeable razor view engine we use
                ViewEngines.Engines.Add(new ThemeableRazorViewEngine());
            }

            //Add some functionality on top of the default ModelMetadataProvider
            ModelMetadataProviders.Current = new SSGMetadataProvider();

            //Registering some regular mvc stuf
            AreaRegistration.RegisterAllAreas();
            if (databaseInstalled &&
                EngineContext.Current.Resolve<SiteInformationSettings>().DisplayMiniProfilerInPublicSite)
            {
                GlobalFilters.Filters.Add(new ProfilingActionFilter());
            }

            //WARNING: Comment this out before installation
            //         Uncomment after installation
            //Registering user profile action filters...
            GlobalFilters.Filters.Add(new UserRoleActionFilter());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new SSGValidatorFactory()));

            //register virtual path provider for embedded views
            var embeddedViewResolver = EngineContext.Current.Resolve<IEmbeddedViewResolver>();
            var embeddedProvider = new EmbeddedViewVirtualPathProvider(embeddedViewResolver.GetEmbeddedViews());
            HostingEnvironment.RegisterVirtualPathProvider(embeddedProvider);

            //mobile device support
            //if (databaseInstalled)
            //{
            //    if (EngineContext.Current.Resolve<SiteInformationSettings>().MobileDevicesSupported)
            //    {
            //        //Enable the mobile detection provider (if enabled)
            //        HttpCapabilitiesBase.BrowserCapabilitiesProvider = new FiftyOne.Foundation.Mobile.Detection.MobileCapabilitiesProvider();
            //    }
            //    else
            //    {
            //        //set BrowserCapabilitiesProvider to null because 51Degrees assembly always sets it to MobileCapabilitiesProvider
            //        //which does not support our browserCaps.config file
            //        HttpCapabilitiesBase.BrowserCapabilitiesProvider = null;
            //    }
            //}

            //start scheduled tasks
            if (databaseInstalled)
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }
        }

        // uncomment to keep alive
        //protected void Application_End()
        //{
        //    var siteInformationSettings = EngineContext.Current.Resolve<SiteInformationSettings>();
        //    var client = new WebClient();
        //    var url = siteInformationSettings.SiteUrl + "keepalive";
        //    client.DownloadString(url);
        //}


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            EnsureDatabaseIsInstalled();

            if (DataSettingsHelper.DatabaseIsInstalled() && 
                EngineContext.Current.Resolve<SiteInformationSettings>().DisplayMiniProfilerInPublicSite)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (DataSettingsHelper.DatabaseIsInstalled() &&
                EngineContext.Current.Resolve<SiteInformationSettings>().DisplayMiniProfilerInPublicSite)
            {
                //stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
                MiniProfiler.Stop();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        { 
            //we don't do it in Application_BeginRequest because a user is not authenticated yet
            SetWorkingCulture();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //disable compression (if enabled). More info - http://stackoverflow.com/questions/3960707/asp-net-mvc-weird-characters-in-error-page
            //log error
            LogException(Server.GetLastError());
        }
        
        protected void EnsureDatabaseIsInstalled()
        {
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            string installUrl = string.Format("{0}install", webHelper.GetSiteLocation());
            if (!webHelper.IsStaticResource(this.Request) &&
                !DataSettingsHelper.DatabaseIsInstalled() &&
                !webHelper.GetThisPageUrl(false).StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Response.Redirect(installUrl);
            }
        }

        protected void SetWorkingCulture()
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            if (webHelper.IsStaticResource(this.Request))
                return;

            //keep alive page requested (we ignore it to prevnt creating a guest user records)
            string keepAliveUrl = string.Format("{0}keepalive", webHelper.GetSiteLocation());
            if (webHelper.GetThisPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;


            if (webHelper.GetThisPageUrl(false).StartsWith(string.Format("{0}admin", webHelper.GetSiteLocation()),
                StringComparison.InvariantCultureIgnoreCase))
            {
                //admin area


                //always set culture to 'en-US'
                //we set culture of admin area to 'en-US' because current implementation of Telerik grid 
                //doesn't work well in other cultures
                //e.g., editing decimal value in russian culture
                var culture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            else
            {
                //public site
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                if (workContext.CurrentUser != null && workContext.WorkingLanguage != null)
                {
                    var culture = new CultureInfo(workContext.WorkingLanguage.LanguageCulture);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;
            
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;
            
            try
            {
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                logger.Error(exc.Message, exc, workContext.CurrentUser);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}