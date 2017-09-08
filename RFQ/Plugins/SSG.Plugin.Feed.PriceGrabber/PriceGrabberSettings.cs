
using SSG.Core.Configuration;

namespace SSG.Plugin.Feed.PriceGrabber
{
    public class PriceGrabberSettings : ISettings
    {
        public int ProductPictureSize { get; set; }

        public int CurrencyId { get; set; }
    }
}