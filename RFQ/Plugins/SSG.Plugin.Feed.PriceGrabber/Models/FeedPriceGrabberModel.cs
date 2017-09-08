using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework;

namespace SSG.Plugin.Feed.PriceGrabber.Models
{
    public class FeedPriceGrabberModel
    {
        public FeedPriceGrabberModel()
        {
            AvailableCurrencies = new List<SelectListItem>();
        }

        [SSGResourceDisplayName("Plugins.Feed.PriceGrabber.ProductPictureSize")]
        public int ProductPictureSize { get; set; }

        [SSGResourceDisplayName("Plugins.Feed.PriceGrabber.Currency")]
        public int CurrencyId { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        public string GenerateFeedResult { get; set; }
    }
}