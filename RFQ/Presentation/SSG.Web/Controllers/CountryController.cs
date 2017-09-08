﻿using System;
using System.Linq;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Caching;
using SSG.Services.Directory;
using SSG.Services.Localization;
using SSG.Web.Infrastructure.Cache;

namespace SSG.Web.Controllers
{
    public partial class CountryController : BaseSSGController
	{
		#region Fields

        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

	    #endregion

		#region Constructors

        public CountryController(ICountryService countryService, 
            IStateProvinceService stateProvinceService, 
            ILocalizationService localizationService, 
            IWorkContext workContext,
            ICacheManager cacheManager)
		{
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
		}

        #endregion

        #region States / provinces

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult GetStatesByCountryId(string countryId, bool addEmptyStateIfRequired)
        {
            //this action method gets called via an ajax request
            if (String.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");
            
            var country = _countryService.GetCountryById(Convert.ToInt32(countryId));
            if (country == null)
                //no country found
                return Json(null, JsonRequestBehavior.AllowGet);

            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, countryId, addEmptyStateIfRequired, _workContext.WorkingLanguage.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var states = _stateProvinceService.GetStateProvincesByCountryId(country.Id).ToList();
                var result = (from s in states
                              select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                              .ToList();

                if (addEmptyStateIfRequired && result.Count == 0)
                    result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                return result;

            });
            
            return Json(cacheModel, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
