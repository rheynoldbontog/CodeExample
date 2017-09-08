using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Boards
{
    public partial class TopicMoveModel : BaseSSGEntityModel
    {
        public TopicMoveModel()
        {
            ForumList = new List<SelectListItem>();
        }

        public int ForumSelected { get; set; }
        public string TopicSeName { get; set; }

        public IEnumerable<SelectListItem> ForumList { get; set; }
    }
}