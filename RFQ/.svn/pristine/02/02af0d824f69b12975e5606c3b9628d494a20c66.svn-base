using System;
using System.Linq;
using SSG.Core;
using SSG.Core.Domain.Common;
using SSG.Services.Topics;

namespace SSG.Services.Seo
{
    /// <summary>
    /// Represents a sitemap generator
    /// </summary>
    public partial class SitemapGenerator : BaseSitemapGenerator, ISitemapGenerator
    {
        private readonly ITopicService _topicService;
        private readonly CommonSettings _commonSettings;
        private readonly IWebHelper _webHelper;

        public SitemapGenerator(ITopicService topicService, CommonSettings commonSettings, IWebHelper webHelper)
        {
            this._topicService = topicService;
            this._commonSettings = commonSettings;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Method that is overridden, that handles creation of child urls.
        /// Use the method WriteUrlLocation() within this method.
        /// </summary>
        protected override void GenerateUrlNodes()
        {
            if (_commonSettings.SitemapIncludeTopics)
            {
                WriteTopics();
            }
        }

        private void WriteTopics()
        {
            var topics = _topicService.GetAllTopics().ToList().FindAll(t => t.IncludeInSitemap);
            foreach (var topic in topics)
            {
                //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
                var url = string.Format("{0}t/{1}", _webHelper.GetSiteLocation(false), topic.SystemName.ToLowerInvariant());
                var updateFrequency = UpdateFrequency.Weekly;
                var updateTime = DateTime.UtcNow;
                WriteUrlLocation(url, updateFrequency, updateTime);
            }
        }
    }
}
