using System.Collections.Generic;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Common
{
    public partial class CurrencySelectorModel : BaseSSGModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public CurrencyModel CurrentCurrency { get; set; }
    }
}