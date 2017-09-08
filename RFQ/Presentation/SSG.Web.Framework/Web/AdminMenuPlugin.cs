using SSG.Core.Plugins;
using Telerik.Web.Mvc.UI;

namespace SSG.Web.Framework.Web
{
    public interface IAdminMenuPlugin : IPlugin
    {
        void BuildMenuItem(MenuItemBuilder menuItemBuilder);
    }
}
