using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Directory;
using SSG.Services.Directory;
using SSG.Services.Helpers;
using SSG.Services.Localization;
using SSG.Services.Media;

namespace SSG.Services.Common
{
    /// <summary>
    /// PDF service
    /// </summary>
    public partial class PdfService : IPdfService
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ICurrencyService _currencyService;
        private readonly IMeasureService _measureService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;

        private readonly CurrencySettings _currencySettings;
        private readonly MeasureSettings _measureSettings;
        private readonly PdfSettings _pdfSettings;
        private readonly SiteInformationSettings _SiteInformationSettings;

        #endregion

        #region Ctor

        public PdfService(ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper, IPriceFormatter priceFormatter,
            ICurrencyService currencyService, IMeasureService measureService,
            IPictureService pictureService, IWebHelper webHelper, 
            CurrencySettings currencySettings,
            MeasureSettings measureSettings, PdfSettings pdfSettings, 
            SiteInformationSettings SiteInformationSettings)
        {
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._priceFormatter = priceFormatter;
            this._currencyService = currencyService;
            this._measureService = measureService;
            this._pictureService = pictureService;
            this._webHelper = webHelper;
            this._currencySettings = currencySettings;
            this._measureSettings = measureSettings;
            this._pdfSettings = pdfSettings;
            this._SiteInformationSettings = SiteInformationSettings;
        }

        #endregion

        #region Utilities

        protected virtual Font GetFont()
        {
            //It was downloaded from http://savannah.gnu.org/projects/freefont
            string fontPath = Path.Combine(_webHelper.MapPath("~/App_Data/Pdf/"), _pdfSettings.FontFileName);
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(baseFont, 10, Font.NORMAL);
            return font;
        }

        #endregion

        #region Methods

        #endregion
    }
}