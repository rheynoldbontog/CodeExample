using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Plugin.Feed.Froogle.Models
{
    public class FeedFroogleModel
    {
        public FeedFroogleModel()
        {
            AvailableCurrencies = new List<SelectListItem>();
            AvailableGoogleCategories = new List<SelectListItem>();
        }

        [SSGResourceDisplayName("Plugins.Feed.Froogle.ProductPictureSize")]
        public int ProductPictureSize { get; set; }

        [SSGResourceDisplayName("Plugins.Feed.Froogle.Currency")]
        public int CurrencyId { get; set; }
        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [SSGResourceDisplayName("Plugins.Feed.Froogle.DefaultGoogleCategory")]
        public string DefaultGoogleCategory { get; set; }
        public IList<SelectListItem> AvailableGoogleCategories { get; set; }

        [SSGResourceDisplayName("Plugins.Feed.Froogle.FtpHostname")]
        public string FtpHostname { get; set; }
        [SSGResourceDisplayName("Plugins.Feed.Froogle.FtpFilename")]
        public string FtpFilename { get; set; }
        [SSGResourceDisplayName("Plugins.Feed.Froogle.FtpUsername")]
        public string FtpUsername { get; set; }
        [SSGResourceDisplayName("Plugins.Feed.Froogle.FtpPassword")]
        public string FtpPassword { get; set; }

        public string GenerateFeedResult { get; set; }
        public string SaveResult { get; set; }



        [SSGResourceDisplayName("Plugins.Feed.Froogle.TaskEnabled")]
        public bool TaskEnabled { get; set; }
        [SSGResourceDisplayName("Plugins.Feed.Froogle.GenerateStaticFileEachMinutes")]
        public int GenerateStaticFileEachMinutes { get; set; }
        [SSGResourceDisplayName("Plugins.Feed.Froogle.StaticFilePath")]
        public string StaticFilePath { get; set; }

        public class GoogleProductModel : BaseSSGModel
        {
            public int ProductVariantId { get; set; }

            [SSGResourceDisplayName("Plugins.Feed.Froogle.Products.ProductName")]
            public string FullProductVariantName { get; set; }

            [SSGResourceDisplayName("Plugins.Feed.Froogle.Products.GoogleCategory")]
            public string GoogleCategory { get; set; }
        }
    }
}