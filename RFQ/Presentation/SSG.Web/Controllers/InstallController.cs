using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Hosting;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Data;
using SSG.Core.Infrastructure;
using SSG.Core.Plugins;
using SSG.Services.Installation;
using SSG.Services.Security;
using SSG.Web.Framework.Security;
using SSG.Web.Infrastructure.Installation;
using SSG.Web.Models.Install;
using MySql.Data.MySqlClient;

namespace SSG.Web.Controllers
{
    public partial class InstallController : BaseSSGController
    {
        #region Fields

        private readonly IInstallationLocalizationService _locService;

        #endregion

        #region Ctor

        public InstallController(IInstallationLocalizationService locService)
        {
            this._locService = locService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Checks if the specified database exists, returns true if database exists
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Returns true if the database exists.</returns>
        [NonAction]
        protected bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                //just try to connect
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        [NonAction]
        protected bool MySQLServerDatabaseExists(string connectionString)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a database on the server.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="collation">Server collation; the default one will be used if not specified</param>
        /// <returns>Error</returns>
        [NonAction]
        protected string CreateDatabase(string connectionString, string collation)
        {
            try
            {
                //parse database name
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;
                //now create connection string to 'master' dabatase. It always exists.
                builder.InitialCatalog = "master";
                var masterCatalogConnectionString = builder.ToString();
                string query = string.Format("CREATE DATABASE [{0}]", databaseName);
                if (!String.IsNullOrWhiteSpace(collation))
                    query = string.Format("{0} COLLATE {1}", query, collation);
                using (var conn = new SqlConnection(masterCatalogConnectionString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();  
                    } 
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format(_locService.GetResource("DatabaseCreationError"), ex.Message);
            }
        }

        [NonAction]
        protected string CreateMySQLDatabase(string connectionString, string databaseName, string collation)
        {
            try
            {
                //parse database name
                var builder = new MySqlConnectionStringBuilder(connectionString);
                //var databaseName = builder.Database;
                var masterCatalogConnectionString = builder.ToString();
                string query = string.Format("CREATE DATABASE IF NOT EXISTS `{0}`", databaseName);
                if (!String.IsNullOrWhiteSpace(collation))
                    query = string.Format("{0} COLLATE {1}", query, collation);
                using (var conn = new MySqlConnection(masterCatalogConnectionString))
                {
                    conn.Open();
                    using (var command = new MySqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format(_locService.GetResource("DatabaseCreationError"), ex.Message);
            }
        }

        /// <summary>
        /// Create contents of connection strings used by the SqlConnection class
        /// </summary>
        /// <param name="trustedConnection">Avalue that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true)</param>
        /// <param name="serverName">The name or network address of the instance of SQL Server to connect to</param>
        /// <param name="databaseName">The name of the database associated with the connection</param>
        /// <param name="userName">The user ID to be used when connecting to SQL Server</param>
        /// <param name="password">The password for the SQL Server account</param>
        /// <param name="timeout">The connection timeout</param>
        /// <returns>Connection string</returns>
        [NonAction]
        protected string CreateConnectionString(bool trustedConnection,
            string serverName, string databaseName, 
            string userName, string password, int timeout = 0)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.IntegratedSecurity = trustedConnection;
            builder.DataSource = serverName;
            builder.InitialCatalog = databaseName;
            if (!trustedConnection)
            {
                builder.UserID = userName;
                builder.Password = password;
            }
            builder.PersistSecurityInfo = false;
            builder.MultipleActiveResultSets = true;
            if (timeout > 0)
            {
                builder.ConnectTimeout = timeout;
            }
            return builder.ConnectionString;
        }

        [NonAction]
        protected string CreateMySQLConnectionString(bool trustedConnection,
            string serverName, string databaseName, 
            string userName, string password, uint timeout = 0, bool attachDatabase = true)
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.IntegratedSecurity = trustedConnection;
            builder.Server = serverName;
            if (attachDatabase)
                builder.Database = databaseName;
            if (!trustedConnection)
            {
                builder.UserID = userName;
                builder.Password = password;
            }
            builder.PersistSecurityInfo = false;
            if (timeout > 0)
            {
                builder.ConnectionTimeout = timeout;
            }
            return builder.ConnectionString;
        }

        [NonAction]
        protected string CreateMySQLConnectionString(bool trustedConnection,
            string serverName, string databaseName,
            string userName, string password, uint port, uint timeout = 0, bool attachDatabase = true)
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.IntegratedSecurity = trustedConnection;
            builder.Server = serverName;
            builder.Port = port;
            if (attachDatabase)
                builder.Database = databaseName;
            if (!trustedConnection)
            {
                builder.UserID = userName;
                builder.Password = password;
            }
            builder.PersistSecurityInfo = false;
            if (timeout > 0)
            {
                builder.ConnectionTimeout = timeout;
            }
            return builder.ConnectionString;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            var model = new InstallModel()
            {
                AdminEmail = "joey.jallorina@teradyne.com",
                //AdminPassword = "admin",
                //ConfirmPassword = "admin",
                InstallSampleData = true,
                DatabaseConnectionString = "",
                // sqlserver settings
                //DataProvider = "sqlserver",
                //SqlAuthenticationType = "sqlauthentication",
                //SqlConnectionInfo = "sqlconnectioninfo_values",
                //SqlServerCreateDatabase = false,
                //UseCustomCollation = false,
                //Collation = "SQL_Latin1_General_CP1_CI_AS",
                // mysql settings
                DataProvider = "mysql",
                MySQLConnectionInfo = "mysqlconnectioninfo_values",
                MySQLServerCreateDatabase = false,
                UseCustomCollation = false,
                Collation = "latin2_general_ci",

            };
            foreach (var lang in _locService.GetAvailableLanguages())
            {
                model.AvailableLanguages.Add(new SelectListItem()
                {
                    Value = Url.Action("ChangeLanguage", "Install", new { language = lang.Code}),
                    Text = lang.Name,
                    Selected = _locService.GetCurrentLanguage().Code == lang.Code,
                });
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(InstallModel model)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            if (model.DatabaseConnectionString != null)
                model.DatabaseConnectionString = model.DatabaseConnectionString.Trim();

            //prepare language list
            foreach (var lang in _locService.GetAvailableLanguages())
            {
                model.AvailableLanguages.Add(new SelectListItem()
                {
                    Value = Url.Action("ChangeLanguage", "Install", new { language = lang.Code }),
                    Text = lang.Name,
                    Selected = _locService.GetCurrentLanguage().Code == lang.Code,
                });
            }

            //SQL Server
            if (model.DataProvider.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.SqlConnectionInfo.Equals("sqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                {
                    //raw connection string
                    if (string.IsNullOrEmpty(model.DatabaseConnectionString))
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringRequired"));

                    try
                    {
                        //try to create connection string
                        new SqlConnectionStringBuilder(model.DatabaseConnectionString);
                    }
                    catch
                    {
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringWrongFormat"));
                    }
                }
                else
                {
                    //values
                    if (string.IsNullOrEmpty(model.SqlServerName))
                        ModelState.AddModelError("", _locService.GetResource("SqlServerNameRequired"));
                    if (string.IsNullOrEmpty(model.SqlDatabaseName))
                        ModelState.AddModelError("", _locService.GetResource("DatabaseNameRequired"));

                    //authentication type
                    if (model.SqlAuthenticationType.Equals("sqlauthentication", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //SQL authentication
                        if (string.IsNullOrEmpty(model.SqlServerUsername))
                            ModelState.AddModelError("", _locService.GetResource("SqlServerUsernameRequired"));
                        if (string.IsNullOrEmpty(model.SqlServerPassword))
                            ModelState.AddModelError("", _locService.GetResource("SqlServerPasswordRequired"));
                    }
                }
            }

            //MySQL Server
            if (model.DataProvider.Equals("mysql", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.MySQLConnectionInfo.Equals("mysqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                {
                    // raw connection string
                    if (string.IsNullOrEmpty(model.MySQLDatabaseConnectionString))
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringRequired"));

                    try
                    {
                        // try to create connection string
                        new MySqlConnectionStringBuilder(model.MySQLDatabaseConnectionString);
                    }
                    catch
                    {
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringWrongFormat"));
                    }
                }
                else
                {
                    // values
                    if (string.IsNullOrEmpty(model.MySQLServerName))
                        ModelState.AddModelError("", _locService.GetResource("MySQLServerNameRequired"));
                    if (string.IsNullOrEmpty(model.MySQLDatabaseName))
                        ModelState.AddModelError("", _locService.GetResource("DatabaseNameRequired"));

                    // authenticate connection
                    if (string.IsNullOrEmpty(model.MySQLServerUsername))
                        ModelState.AddModelError("", _locService.GetResource("MySQLServerUsernameRequired"));
                    if (string.IsNullOrEmpty(model.MySQLServerPassword))
                        ModelState.AddModelError("", _locService.GetResource("MySQLServerPasswordRequired"));
                }
            }



            //Consider granting access rights to the resource to the ASP.NET request identity. 
            //ASP.NET has a base process identity 
            //(typically {MACHINE}\ASPNET on IIS 5 or Network Service on IIS 6 and IIS 7, 
            //and the configured application pool identity on IIS 7.5) that is used if the application is not impersonating.
            //If the application is impersonating via <identity impersonate="true"/>, 
            //the identity will be the anonymous user (typically IUSR_MACHINENAME) or the authenticated request user.
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //validate permissions
            var dirsToCheck = FilePermissionHelper.GetDirectoriesWrite(webHelper);
            foreach (string dir in dirsToCheck)
                if (!FilePermissionHelper.CheckPermissions(dir, false, true, true, false))
                    ModelState.AddModelError("", string.Format(_locService.GetResource("ConfigureDirectoryPermissions"), WindowsIdentity.GetCurrent().Name, dir));

            var filesToCheck = FilePermissionHelper.GetFilesWrite(webHelper);
            foreach (string file in filesToCheck)
                if (!FilePermissionHelper.CheckPermissions(file, false, true, true, true))
                    ModelState.AddModelError("", string.Format(_locService.GetResource("ConfigureFilePermissions"), WindowsIdentity.GetCurrent().Name, file));
            
            if (ModelState.IsValid)
            {
                var settingsManager = new DataSettingsManager();
                try
                {
                    string connectionString = null;

                    //SQL Server
                    if (model.DataProvider.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (model.SqlConnectionInfo.Equals("sqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //raw connection string

                            //we know that MARS option is required when using Entity Framework
                            //let's ensure that it's specified
                            var sqlCsb = new SqlConnectionStringBuilder(model.DatabaseConnectionString);
                            sqlCsb.MultipleActiveResultSets = true;
                            connectionString = sqlCsb.ToString();
                        }
                        else
                        {
                            //values
                            connectionString = CreateConnectionString(model.SqlAuthenticationType == "windowsauthentication",
                                model.SqlServerName, model.SqlDatabaseName,
                                model.SqlServerUsername, model.SqlServerPassword);
                        }
                        
                        if (model.SqlServerCreateDatabase)
                        {
                            if (!SqlServerDatabaseExists(connectionString))
                            {
                                //create database
                                var collation = model.UseCustomCollation ? model.Collation : "";
                                var errorCreatingDatabase = CreateDatabase(connectionString, collation);
                                if (!String.IsNullOrEmpty(errorCreatingDatabase))
                                    throw new Exception(errorCreatingDatabase);
                                else
                                {
                                    //Database cannot be created sometimes. Weird! Seems to be Entity Framework issue
                                    //that's just wait 3 seconds
                                    Thread.Sleep(3000);
                                }
                            }
                        }
                        else
                        {
                            //check whether database exists
                            if (!SqlServerDatabaseExists(connectionString))
                                throw new Exception(_locService.GetResource("DatabaseNotExists"));
                        }
                    }
                    // MySQL
                    else if (model.DataProvider.Equals("mysql", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (model.MySQLConnectionInfo.Equals("mysqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // raw connection string
                            var mySqlCsb = new MySqlConnectionStringBuilder(model.MySQLDatabaseConnectionString);
                            // No MARS support in MySQL, make sure POOLING is ON
                            mySqlCsb.Pooling = true;
                            connectionString = mySqlCsb.ToString();
                        }
                        else
                        {
                            // values
                            connectionString = CreateMySQLConnectionString(false,
                                model.MySQLServerName, model.MySQLDatabaseName, 
                                model.MySQLServerUsername, model.MySQLServerPassword);
                        }

                        if (model.MySQLServerCreateDatabase)
                        {
                            //if (!MySQLServerDatabaseExists(connectionString))
                            //{
                                // Attach the database
                            //if (model.MySQLConnectionInfo.Equals("mysqlconnectioninfo_values", StringComparison.InvariantCultureIgnoreCase))
                            string pseudoConnectionString = null;

                            if (!MySQLServerDatabaseExists(connectionString))
                            {
                                if (model.MySQLConnectionInfo.Equals("mysqlconnectioninfo_values", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    pseudoConnectionString = CreateMySQLConnectionString(false,
                                        model.MySQLServerName, model.MySQLDatabaseName,
                                        model.MySQLServerUsername, model.MySQLServerPassword, 0, false);
                                }
                                else
                                {
                                    pseudoConnectionString = connectionString;
                                }
                                // Create database
                                var collation = model.UseCustomCollation ? model.Collation : "";
                                var errorCreatingDatabase = CreateMySQLDatabase(pseudoConnectionString, model.MySQLDatabaseName, collation);
                                if (!String.IsNullOrEmpty(errorCreatingDatabase))
                                    throw new Exception(errorCreatingDatabase);
                                else
                                {
                                    // For some reason
                                    Thread.Sleep(5000);
                                }
                            }
                            //}
                        }
                    }
                    else
                    {
                        //SQL CE
                        string databaseFileName = "SSG.Db.sdf";
                        string databasePath = @"|DataDirectory|\" + databaseFileName;
                        connectionString = "Data Source=" + databasePath + ";Persist Security Info=False";

                        //drop database if exists
                        string databaseFullPath = HostingEnvironment.MapPath("~/App_Data/") + databaseFileName;
                        if (System.IO.File.Exists(databaseFullPath))
                        {
                            System.IO.File.Delete(databaseFullPath);
                        }
                    }

                    //save settings
                    var dataProvider = model.DataProvider;
                    var settings = new DataSettings()
                    {
                        DataProvider = dataProvider,
                        DataConnectionString = connectionString
                    };
                    settingsManager.SaveSettings(settings);

                    //init data provider
                    var dataProviderInstance = EngineContext.Current.Resolve<BaseDataProviderManager>().LoadDataProvider();
                    dataProviderInstance.InitDatabase();
                    
                    
                    //now resolve installation service
                    var installationService = EngineContext.Current.Resolve<IInstallationService>();
                    installationService.InstallData(model.AdminEmail, model.AdminPassword, model.InstallSampleData);

                    //reset cache
                    DataSettingsHelper.ResetCache();

                    //install plugins
                    PluginManager.MarkAllPluginsAsUninstalled();
                    var pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
                    var plugins = pluginFinder.GetPlugins<IPlugin>(false)
                        .ToList()
                        .OrderBy(x => x.PluginDescriptor.Group)
                        .ThenBy(x => x.PluginDescriptor.DisplayOrder)
                        .ToList();
                    foreach (var plugin in plugins)
                    {
                        plugin.Install();
                    }
                    
                    //register default permissions
                    //var permissionProviders = EngineContext.Current.Resolve<ITypeFinder>().FindClassesOfType<IPermissionProvider>();
                    var permissionProviders = new List<Type>();
                    permissionProviders.Add(typeof(StandardPermissionProvider));
                    foreach (var providerType in permissionProviders)
                    {
                        dynamic provider = Activator.CreateInstance(providerType);
                        EngineContext.Current.Resolve<IPermissionService>().InstallPermissions(provider);
                    }

                    //restart application
                    webHelper.RestartAppDomain();

                    //Redirect to home page
                    return RedirectToRoute("HomePage");
                }
                catch (Exception exception)
                {
                    //reset cache
                    DataSettingsHelper.ResetCache();

                    //clear provider settings if something got wrong
                    settingsManager.SaveSettings(new DataSettings
                    {
                        DataProvider = null,
                        DataConnectionString = null
                    });

                    ModelState.AddModelError("", string.Format(_locService.GetResource("SetupFailed"), exception.Message));
                }
            }
            return View(model);
        }

        public virtual ActionResult ChangeLanguage(string language)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            _locService.SaveCurrentLanguage(language);

            //Reload the page);
            return RedirectToAction("Index", "Install");
        }

        public virtual ActionResult RestartInstall()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");
            
            //restart application
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            webHelper.RestartAppDomain();

            //Redirect to home page
            return RedirectToRoute("HomePage");
        }

        #endregion
    }
}
