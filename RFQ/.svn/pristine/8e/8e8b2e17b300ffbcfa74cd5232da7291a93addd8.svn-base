using System.Collections.Generic;
using SSG.Core.Domain.Users;
using SSG.Core.Domain.Security;

namespace SSG.Services.Security
{
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord AllowUserImpersonation = new PermissionRecord { Name = "Admin area. Allow User Impersonation", SystemName = "AllowUserImpersonation", Category = "Users" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Admin area. Manage Users", SystemName = "ManageUsers", Category = "Users" };
        public static readonly PermissionRecord ManageUserRoles = new PermissionRecord { Name = "Admin area. Manage User Roles", SystemName = "ManageUserRoles", Category = "Users" };
        public static readonly PermissionRecord ManageCampaigns = new PermissionRecord { Name = "Admin area. Manage Campaigns", SystemName = "ManageCampaigns", Category = "Promo" };
        public static readonly PermissionRecord ManageNewsletterSubscribers = new PermissionRecord { Name = "Admin area. Manage Newsletter Subscribers", SystemName = "ManageNewsletterSubscribers", Category = "Promo" };
        public static readonly PermissionRecord ManagePolls = new PermissionRecord { Name = "Admin area. Manage Polls", SystemName = "ManagePolls", Category = "Content Management" };
        public static readonly PermissionRecord ManageNews = new PermissionRecord { Name = "Admin area. Manage News", SystemName = "ManageNews", Category = "Content Management" };
        public static readonly PermissionRecord ManageWidgets = new PermissionRecord { Name = "Admin area. Manage Widgets", SystemName = "ManageWidgets", Category = "Content Management" };
        public static readonly PermissionRecord ManageTopics = new PermissionRecord { Name = "Admin area. Manage Topics", SystemName = "ManageTopics", Category = "Content Management" };
        public static readonly PermissionRecord ManageForums = new PermissionRecord { Name = "Admin area. Manage Forums", SystemName = "ManageForums", Category = "Content Management" };
        public static readonly PermissionRecord ManageMessageTemplates = new PermissionRecord { Name = "Admin area. Manage Message Templates", SystemName = "ManageMessageTemplates", Category = "Content Management" };
        public static readonly PermissionRecord ManageCountries = new PermissionRecord { Name = "Admin area. Manage Countries", SystemName = "ManageCountries", Category = "Configuration" };
        public static readonly PermissionRecord ManageLanguages = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Configuration" };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Admin area. Manage Settings", SystemName = "ManageSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManagePaymentMethods = new PermissionRecord { Name = "Admin area. Manage Payment Methods", SystemName = "ManagePaymentMethods", Category = "Configuration" };
        public static readonly PermissionRecord ManageExternalAuthenticationMethods = new PermissionRecord { Name = "Admin area. Manage External Authentication Methods", SystemName = "ManageExternalAuthenticationMethods", Category = "Configuration" };
        public static readonly PermissionRecord ManageCurrencies = new PermissionRecord { Name = "Admin area. Manage Currencies", SystemName = "ManageCurrencies", Category = "Configuration" };
        public static readonly PermissionRecord ManageMeasures = new PermissionRecord { Name = "Admin area. Manage Measures", SystemName = "ManageMeasures", Category = "Configuration" };
        public static readonly PermissionRecord ManageActivityLog = new PermissionRecord { Name = "Admin area. Manage Activity Log", SystemName = "ManageActivityLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageAcl = new PermissionRecord { Name = "Admin area. Manage ACL", SystemName = "ManageACL", Category = "Configuration" };
        public static readonly PermissionRecord ManageEmailAccounts = new PermissionRecord { Name = "Admin area. Manage Email Accounts", SystemName = "ManageEmailAccounts", Category = "Configuration" };
        public static readonly PermissionRecord ManagePlugins = new PermissionRecord { Name = "Admin area. Manage Plugins", SystemName = "ManagePlugins", Category = "Configuration" };
        public static readonly PermissionRecord ManageSystemLog = new PermissionRecord { Name = "Admin area. Manage System Log", SystemName = "ManageSystemLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageMessageQueue = new PermissionRecord { Name = "Admin area. Manage Message Queue", SystemName = "ManageMessageQueue", Category = "Configuration" };
        public static readonly PermissionRecord ManageMaintenance = new PermissionRecord { Name = "Admin area. Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };
        public static readonly PermissionRecord UploadPictures = new PermissionRecord { Name = "Admin area. Upload Pictures", SystemName = "UploadPictures", Category = "Configuration" };


        //public site permissions
        public static readonly PermissionRecord DisplayPrices = new PermissionRecord { Name = "Public site. Display Prices", SystemName = "DisplayPrices", Category = "PublicSite" };
        public static readonly PermissionRecord PublicSiteAllowNavigation = new PermissionRecord { Name = "Public site. Allow navigation", SystemName = "PublicSiteAllowNavigation", Category = "PublicSite" };
        public static readonly PermissionRecord ViewOrders = new PermissionRecord { Name = "Public site. View orders", SystemName = "ViewOrders", Category = "PublicSite" };
        public static readonly PermissionRecord CreateOrders = new PermissionRecord { Name = "Public site. Create orders", SystemName = "CreateOrders", Category = "PublicSite" };
        public static readonly PermissionRecord UpdateOrders = new PermissionRecord { Name = "Public site. Update orders", SystemName = "UpdateOrders", Category = "PublicSite" };

        
        // Application permissions here

        #region maintenance

        public static readonly PermissionRecord MaintenanceNavigation = new PermissionRecord { Name = "Maintenance Navigation", SystemName = "MaintenanceNavigation", Category = "Maintenance" };

        #endregion

        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[] 
            {
                AccessAdminPanel,
                AllowUserImpersonation,
                ManageUsers,
                ManageUserRoles,
                ManageCampaigns,
                ManageNewsletterSubscribers,
                ManagePolls,
                ManageNews,
                ManageWidgets,
                ManageTopics,
                ManageForums,
                ManageMessageTemplates,
                ManageCountries,
                ManageLanguages,
                ManageSettings,
                ManagePaymentMethods,
                ManageExternalAuthenticationMethods,
                ManageCurrencies,
                ManageMeasures,
                ManageActivityLog,
                ManageAcl,
                ManageEmailAccounts,
                ManagePlugins,
                ManageSystemLog,
                ManageMessageQueue,
                ManageMaintenance,
                UploadPictures,
                DisplayPrices,
                PublicSiteAllowNavigation,
                MaintenanceNavigation,
            };
        }

        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[] 
            {
                new DefaultPermissionRecord 
                {
                    UserRoleSystemName = SystemUserRoleNames.Administrators,
                    PermissionRecords = new[] 
                    {
                        AccessAdminPanel,
                        AllowUserImpersonation,
                        ManageUsers,
                        ManageUserRoles,
                        ManageCampaigns,
                        ManageNewsletterSubscribers,
                        ManagePolls,
                        ManageNews,
                        ManageWidgets,
                        ManageTopics,
                        ManageForums,
                        ManageMessageTemplates,
                        ManageCountries,
                        ManageLanguages,
                        ManageSettings,
                        ManagePaymentMethods,
                        ManageExternalAuthenticationMethods,
                        ManageCurrencies,
                        ManageMeasures,
                        ManageActivityLog,
                        ManageAcl,
                        ManageEmailAccounts,
                        ManagePlugins,
                        ManageSystemLog,
                        ManageMessageQueue,
                        ManageMaintenance,
                        UploadPictures,
                        DisplayPrices,
                        PublicSiteAllowNavigation,
                        MaintenanceNavigation,
                    }
                },
                new DefaultPermissionRecord 
                {
                    UserRoleSystemName = SystemUserRoleNames.ForumModerators,
                    PermissionRecords = new[] 
                    {
                        DisplayPrices,
                        PublicSiteAllowNavigation,
                    }
                },
                new DefaultPermissionRecord 
                {
                    UserRoleSystemName = SystemUserRoleNames.Guests,
                    PermissionRecords = new[] 
                    {
                        DisplayPrices,
                        PublicSiteAllowNavigation,
                    }
                },
                new DefaultPermissionRecord 
                {
                    UserRoleSystemName = SystemUserRoleNames.Registered,
                    PermissionRecords = new[] 
                    {
                        DisplayPrices,
                        PublicSiteAllowNavigation,
                    }
                },
            };
        }
    }
}