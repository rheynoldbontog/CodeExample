using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework;

namespace SSG.Plugin.Feed.Become.Models
{
    public class FeedBecomeModel
    {
        public FeedBecomeModel()
        {
            AvailableCurrencies = new List<SelectListItem>();
        }

        [SSGResourceDisplayName("Plugins.Feed.Become.ProductPictureSize")]
        public int ProductPictureSize { get; set; }

        [SSGResourceDisplayName("Plugins.Feed.Become.Currency")]
        public int CurrencyId { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        public string GenerateFeedResult { get; set; }
    }
}