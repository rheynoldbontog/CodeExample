using System.Net;
using SSG.Core.Domain;
using SSG.Services.Tasks;

namespace SSG.Services.Common
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : ITask
    {
        private readonly SiteInformationSettings _siteInformationSettings;
        public KeepAliveTask(SiteInformationSettings siteInformationSettings)
        {
            this._siteInformationSettings = siteInformationSettings;
        }
        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            string url = _siteInformationSettings.SiteUrl + "keepalive";
            using (var wc = new WebClient())
            {
                wc.DownloadString(url);
            }
        }
    }
}
