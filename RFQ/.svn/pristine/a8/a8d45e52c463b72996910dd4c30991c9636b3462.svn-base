using System;
using System.Collections.Generic;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseSSGModel, ICloneable
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }

        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return this.MemberwiseClone();
        }
    }
}