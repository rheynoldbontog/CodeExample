using System.Collections.Generic;
using SSG.Web.Framework.Mvc;
using SSG.Web.Models.Topics;

namespace SSG.Web.Models.Common
{
    public partial class SitemapModel : BaseSSGModel
    {
        public SitemapModel()
        {
            Topics = new List<TopicModel>();
        }
        public IList<TopicModel> Topics { get; set; }
    }
}