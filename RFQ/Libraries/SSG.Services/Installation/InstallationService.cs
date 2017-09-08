using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using SSG.Core;
using SSG.Core.Configuration;
using SSG.Core.Data;
using SSG.Core.Domain;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Localization;
using SSG.Core.Domain.Logging;
using SSG.Core.Domain.Media;
using SSG.Core.Domain.Messages;
using SSG.Core.Domain.News;
using SSG.Core.Domain.Polls;
using SSG.Core.Domain.Security;
using SSG.Core.Domain.Tasks;
using SSG.Core.Domain.Topics;
using SSG.Core.Domain.Users;
using SSG.Core.Infrastructure;
using SSG.Core.IO;
using SSG.Services.Common;
using SSG.Services.Helpers;
using SSG.Services.Localization;
using SSG.Services.Users;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.Installation
{
	public partial class InstallationService : IInstallationService
	{
		#region Fields

		private readonly IRepository<MeasureDimension> _measureDimensionRepository;
		private readonly IRepository<MeasureWeight> _measureWeightRepository;
		private readonly IRepository<Language> _languageRepository;
		private readonly IRepository<Currency> _currencyRepository;
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<UserRole> _userRoleRepository;
		private readonly IRepository<EmailAccount> _emailAccountRepository;
		private readonly IRepository<MessageTemplate> _messageTemplateRepository;
		private readonly IRepository<ForumGroup> _forumGroupRepository;
		private readonly IRepository<Forum> _forumRepository;
		private readonly IRepository<Country> _countryRepository;
		private readonly IRepository<StateProvince> _stateProvinceRepository;
		private readonly IRepository<Topic> _topicRepository;
		private readonly IRepository<NewsItem> _newsItemRepository;
		private readonly IRepository<Poll> _pollRepository;
		private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
		private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly IRepository<RFQStatus> _rfqStatusRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<RFQLineType> _rfqLineTypeRepository;
        private readonly IRepository<RFQLineFormType> _rfqLineFormTypeRepository;
        private readonly IRepository<EquipmentPurchaseType> _equipmentPurchaseTypeRepository;
        private readonly IRepository<EquipmentCalibrationType> _equipmentCalibrationTypeRepository;
        private readonly IRepository<UOM> _uomRepository;
        private readonly IRepository<RFQLineStatus> _rfqLineStatusRepository;
        private readonly IGenericAttributeService _genericAttributeService;
		private readonly IWebHelper _webHelper;

		#endregion

		#region Ctor

		public InstallationService(IRepository<MeasureDimension> measureDimensionRepository,
			IRepository<MeasureWeight> measureWeightRepository,
			IRepository<Language> languageRepository,
			IRepository<Currency> currencyRepository,
			IRepository<User> userRepository,
			IRepository<UserRole> userRoleRepository,
			IRepository<EmailAccount> emailAccountRepository,
			IRepository<MessageTemplate> messageTemplateRepository,
			IRepository<ForumGroup> forumGroupRepository,
			IRepository<Forum> forumRepository,
			IRepository<Country> countryRepository,
			IRepository<StateProvince> stateProvinceRepository,
			IRepository<Topic> topicRepository,
			IRepository<NewsItem> newsItemRepository,
			IRepository<Poll> pollRepository,
			IRepository<ActivityLogType> activityLogTypeRepository,
			IRepository<ScheduleTask> scheduleTaskRepository,
            IRepository<RFQStatus> rfqStatusRepository,
            IRepository<Department> departmentRepository,
            IRepository<RFQLineType> rfqLineTypeRepository,
            IRepository<RFQLineFormType> rfqLineFormTypeRepository,
            IRepository<EquipmentPurchaseType> equipmentPurchaseTypeRepository,
            IRepository<EquipmentCalibrationType> equipmentCalibrationTypeRepository,
            IRepository<UOM> uomRepository,
            IRepository<RFQLineStatus> rfqLineStatusRepository,
			IGenericAttributeService genericAttributeService,
			IWebHelper webHelper)
		{
			this._measureDimensionRepository = measureDimensionRepository;
			this._measureWeightRepository = measureWeightRepository;
			this._languageRepository = languageRepository;
			this._currencyRepository = currencyRepository;
			this._userRepository = userRepository;
			this._userRoleRepository = userRoleRepository;
			this._emailAccountRepository = emailAccountRepository;
			this._messageTemplateRepository = messageTemplateRepository;
			this._forumGroupRepository = forumGroupRepository;
			this._forumRepository = forumRepository;
			this._countryRepository = countryRepository;
			this._stateProvinceRepository = stateProvinceRepository;
			this._topicRepository = topicRepository;
			this._newsItemRepository = newsItemRepository;
			this._pollRepository = pollRepository;
			this._activityLogTypeRepository = activityLogTypeRepository;
			this._scheduleTaskRepository = scheduleTaskRepository;
            this._rfqStatusRepository = rfqStatusRepository;
            this._departmentRepository = departmentRepository;
            this._rfqLineTypeRepository = rfqLineTypeRepository;
            this._rfqLineFormTypeRepository = rfqLineFormTypeRepository;
            this._equipmentPurchaseTypeRepository = equipmentPurchaseTypeRepository;
            this._equipmentCalibrationTypeRepository = equipmentCalibrationTypeRepository;
            this._uomRepository = uomRepository;
            this._rfqLineStatusRepository = rfqLineStatusRepository;
			this._genericAttributeService = genericAttributeService;
			this._webHelper = webHelper;
		}

		#endregion

		#region Classes

		private class LocaleStringResourceParent : LocaleStringResource
		{
			public LocaleStringResourceParent(XmlNode localStringResource, string nameSpace = "")
			{
				Namespace = nameSpace;
				var resNameAttribute = localStringResource.Attributes["Name"];
				var resValueNode = localStringResource.SelectSingleNode("Value");

				if (resNameAttribute == null)
				{
					throw new SSGException("All language resources must have an attribute Name=\"Value\".");
				}
				var resName = resNameAttribute.Value.Trim();
				if (string.IsNullOrEmpty(resName))
				{
					throw new SSGException("All languages resource attributes 'Name' must have a value.'");
				}
				ResourceName = resName;

				if (resValueNode == null || string.IsNullOrEmpty(resValueNode.InnerText.Trim()))
				{
					IsPersistable = false;
				}
				else
				{
					IsPersistable = true;
					ResourceValue = resValueNode.InnerText.Trim();
				}

				foreach (XmlNode childResource in localStringResource.SelectNodes("Children/LocaleResource"))
				{
					ChildLocaleStringResources.Add(new LocaleStringResourceParent(childResource, NameWithNamespace));
				}
			}
			public string Namespace { get; set; }
			public IList<LocaleStringResourceParent> ChildLocaleStringResources = new List<LocaleStringResourceParent>();

			public bool IsPersistable { get; set; }

			public string NameWithNamespace
			{
				get
				{
					var newNamespace = Namespace;
					if (!string.IsNullOrEmpty(newNamespace))
					{
						newNamespace += ".";
					}
					return newNamespace + ResourceName;
				}
			}
		}

		private class ComparisonComparer<T> : IComparer<T>, IComparer
		{
			private readonly Comparison<T> _comparison;

			public ComparisonComparer(Comparison<T> comparison)
			{
				_comparison = comparison;
			}

			public int Compare(T x, T y)
			{
				return _comparison(x, y);
			}

			public int Compare(object o1, object o2)
			{
				return _comparison((T)o1, (T)o2);
			}
		}

		#endregion

		#region Utilities

		private void RecursivelyWriteResource(LocaleStringResourceParent resource, XmlWriter writer)
		{
			//The value isn't actually used, but the name is used to create a namespace.
			if (resource.IsPersistable)
			{
				writer.WriteStartElement("LocaleResource", "");

				writer.WriteStartAttribute("Name", "");
				writer.WriteString(resource.NameWithNamespace);
				writer.WriteEndAttribute();

				writer.WriteStartElement("Value", "");
				writer.WriteString(resource.ResourceValue);
				writer.WriteEndElement();

				writer.WriteEndElement();
			}

			foreach (var child in resource.ChildLocaleStringResources)
			{
				RecursivelyWriteResource(child, writer);
			}

		}

		private void RecursivelySortChildrenResource(LocaleStringResourceParent resource)
		{
			ArrayList.Adapter((IList)resource.ChildLocaleStringResources).Sort(new InstallationService.ComparisonComparer<LocaleStringResourceParent>((x1, x2) => x1.ResourceName.CompareTo(x2.ResourceName)));

			foreach (var child in resource.ChildLocaleStringResources)
			{
				RecursivelySortChildrenResource(child);
			}

		}

		protected virtual void InstallMeasures()
		{
			var measureDimensions = new List<MeasureDimension>()
			{
				new MeasureDimension()
				{
					Name = "inch(es)",
					SystemKeyword = "inches",
					Ratio = 1M,
					DisplayOrder = 1,
				},
				new MeasureDimension()
				{
					Name = "feet",
					SystemKeyword = "feet",
					Ratio = 0.08333333M,
					DisplayOrder = 2,
				},
				new MeasureDimension()
				{
					Name = "meter(s)",
					SystemKeyword = "meters",
					Ratio = 0.0254M,
					DisplayOrder = 3,
				},
				new MeasureDimension()
				{
					Name = "millimetre(s)",
					SystemKeyword = "millimetres",
					Ratio = 25.4M,
					DisplayOrder = 4,
				}
			};

			measureDimensions.ForEach(x => _measureDimensionRepository.Insert(x));

			var measureWeights = new List<MeasureWeight>()
			{
				new MeasureWeight()
				{
					Name = "ounce(s)",
					SystemKeyword = "ounce",
					Ratio = 16M,
					DisplayOrder = 1,
				},
				new MeasureWeight()
				{
					Name = "lb(s)",
					SystemKeyword = "lb",
					Ratio = 1M,
					DisplayOrder = 2,
				},
				new MeasureWeight()
				{
					Name = "kg(s)",
					SystemKeyword = "kg",
					Ratio = 0.45359237M,
					DisplayOrder = 3,
				},
				new MeasureWeight()
				{
					Name = "gram(s)",
					SystemKeyword = "grams",
					Ratio = 453.59237M,
					DisplayOrder = 4,
				}
			};

			measureWeights.ForEach(x => _measureWeightRepository.Insert(x));
		}

		protected virtual void InstallLanguages()
		{
			var language = new Language
			{
				Name = "English",
				LanguageCulture = "en-US",
				UniqueSeoCode = "en",
				FlagImageFileName = "us.png",
				Published = true,
				DisplayOrder = 1
			};
			_languageRepository.Insert(language);
		}

		protected virtual void InstallLocaleResources()
		{
			//'English' language
			var language = _languageRepository.Table.Where(l => l.Name == "English").Single();

			//save resoureces
			foreach (var filePath in System.IO.Directory.EnumerateFiles(_webHelper.MapPath("~/App_Data/Localization/"), "*.ssgres.xml", SearchOption.TopDirectoryOnly))
			{
				#region Parse resource files (with <Children> elements)
				//read and parse original file with resources (with <Children> elements)

				var originalXmlDocument = new XmlDocument();
				originalXmlDocument.Load(filePath);

				var resources = new List<LocaleStringResourceParent>();

				foreach (XmlNode resNode in originalXmlDocument.SelectNodes(@"//Language/LocaleResource"))
					resources.Add(new LocaleStringResourceParent(resNode));

				resources.Sort((x1, x2) => x1.ResourceName.CompareTo(x2.ResourceName));

				foreach (var resource in resources)
					RecursivelySortChildrenResource(resource);

				var sb = new StringBuilder();
				var writer = XmlWriter.Create(sb);
				writer.WriteStartDocument();
				writer.WriteStartElement("Language", "");

				writer.WriteStartAttribute("Name", "");
				writer.WriteString(originalXmlDocument.SelectSingleNode(@"//Language").Attributes["Name"].InnerText.Trim());
				writer.WriteEndAttribute();

				foreach (var resource in resources)
					RecursivelyWriteResource(resource, writer);

				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush();

				var parsedXml = sb.ToString();
				#endregion

				//now we have a parsed XML file (the same structure as exported language packs)
				//let's save resources
				var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
				localizationService.ImportResourcesFromXml(language, parsedXml);
			}

		}

		protected virtual void InstallCurrencies()
		{
			var currencies = new List<Currency>()
			{
				new Currency
				{
					Name = "US Dollar",
					CurrencyCode = "USD",
					Rate = 1,
					DisplayLocale = "en-US",
					CustomFormatting = "",
					Published = true,
					DisplayOrder = 1,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
                new Currency
                {
                    Name = "Philippine Peso",
                    CurrencyCode = "PHP",
                    Rate = 50,
                    DisplayLocale = "en-US",
                    CustomFormatting = "",
					Published = true,
					DisplayOrder = 1,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
                },
				new Currency
				{
					Name = "Australian Dollar",
					CurrencyCode = "AUD",
					Rate = 0.94M,
					DisplayLocale = "en-AU",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 2,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "British Pound",
					CurrencyCode = "GBP",
					Rate = 0.61M,
					DisplayLocale = "en-GB",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 3,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Canadian Dollar",
					CurrencyCode = "CAD",
					Rate = 0.98M,
					DisplayLocale = "en-CA",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 4,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Chinese Yuan Renminbi",
					CurrencyCode = "CNY",
					Rate = 6.48M,
					DisplayLocale = "zh-CN",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 5,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Euro",
					CurrencyCode = "EUR",
					Rate = 0.79M,
					DisplayLocale = "",
					//CustomFormatting = "�0.00",
					CustomFormatting = string.Format("{0}0.00", "\u20ac"),
					Published = true,
					DisplayOrder = 6,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Hong Kong Dollar",
					CurrencyCode = "HKD",
					Rate = 7.75M,
					DisplayLocale = "zh-HK",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 7,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Japanese Yen",
					CurrencyCode = "JPY",
					Rate = 80.07M,
					DisplayLocale = "ja-JP",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 8,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Russian Rouble",
					CurrencyCode = "RUB",
					Rate = 27.7M,
					DisplayLocale = "ru-RU",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 9,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Swedish Krona",
					CurrencyCode = "SEK",
					Rate = 6.19M,
					DisplayLocale = "sv-SE",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 10,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Romanian Leu",
					CurrencyCode = "RON",
					Rate = 2.85M,
					DisplayLocale = "ro-RO",
					CustomFormatting = "",
					Published = false,
					DisplayOrder = 11,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
			};
			currencies.ForEach(c => _currencyRepository.Insert(c));
		}

		protected virtual void InstallCountriesAndStates()
		{
			var cUsa = new Country
			{
				Name = "United States",
				TwoLetterIsoCode = "US",
				ThreeLetterIsoCode = "USA",
				NumericIsoCode = 840,
				DisplayOrder = 1,
				Published = true,
			};
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "AA (Armed Forces Americas)",
				Abbreviation = "AA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "AE (Armed Forces Europe)",
				Abbreviation = "AE",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Alabama",
				Abbreviation = "AL",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Alaska",
				Abbreviation = "AK",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "American Samoa",
				Abbreviation = "AS",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "AP (Armed Forces Pacific)",
				Abbreviation = "AP",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Arizona",
				Abbreviation = "AZ",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Arkansas",
				Abbreviation = "AR",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "California",
				Abbreviation = "CA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Colorado",
				Abbreviation = "CO",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Connecticut",
				Abbreviation = "CT",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Delaware",
				Abbreviation = "DE",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "District of Columbia",
				Abbreviation = "DC",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Federated States of Micronesia",
				Abbreviation = "FM",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Florida",
				Abbreviation = "FL",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Georgia",
				Abbreviation = "GA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Guam",
				Abbreviation = "GU",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Hawaii",
				Abbreviation = "HI",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Idaho",
				Abbreviation = "ID",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Illinois",
				Abbreviation = "IL",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Indiana",
				Abbreviation = "IN",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Iowa",
				Abbreviation = "IA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Kansas",
				Abbreviation = "KS",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Kentucky",
				Abbreviation = "KY",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Louisiana",
				Abbreviation = "LA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Maine",
				Abbreviation = "ME",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Marshall Islands",
				Abbreviation = "MH",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Maryland",
				Abbreviation = "MD",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Massachusetts",
				Abbreviation = "MA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Michigan",
				Abbreviation = "MI",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Minnesota",
				Abbreviation = "MN",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Mississippi",
				Abbreviation = "MS",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Missouri",
				Abbreviation = "MO",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Montana",
				Abbreviation = "MT",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Nebraska",
				Abbreviation = "NE",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Nevada",
				Abbreviation = "NV",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "New Hampshire",
				Abbreviation = "NH",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "New Jersey",
				Abbreviation = "NJ",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "New Mexico",
				Abbreviation = "NM",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "New York",
				Abbreviation = "NY",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "North Carolina",
				Abbreviation = "NC",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "North Dakota",
				Abbreviation = "ND",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Northern Mariana Islands",
				Abbreviation = "MP",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Ohio",
				Abbreviation = "OH",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Oklahoma",
				Abbreviation = "OK",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Oregon",
				Abbreviation = "OR",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Palau",
				Abbreviation = "PW",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Pennsylvania",
				Abbreviation = "PA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Puerto Rico",
				Abbreviation = "PR",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Rhode Island",
				Abbreviation = "RI",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "South Carolina",
				Abbreviation = "SC",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "South Dakota",
				Abbreviation = "SD",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Tennessee",
				Abbreviation = "TN",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Texas",
				Abbreviation = "TX",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Utah",
				Abbreviation = "UT",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Vermont",
				Abbreviation = "VT",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Virgin Islands",
				Abbreviation = "VI",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Virginia",
				Abbreviation = "VA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Washington",
				Abbreviation = "WA",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "West Virginia",
				Abbreviation = "WV",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Wisconsin",
				Abbreviation = "WI",
				Published = true,
				DisplayOrder = 1,
			});
			cUsa.StateProvinces.Add(new StateProvince()
			{
				Name = "Wyoming",
				Abbreviation = "WY",
				Published = true,
				DisplayOrder = 1,
			});
			var cCanada = new Country
			{
				Name = "Canada",
				TwoLetterIsoCode = "CA",
				ThreeLetterIsoCode = "CAN",
				NumericIsoCode = 124,
				DisplayOrder = 2,
				Published = true,
			};
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Alberta",
				Abbreviation = "AB",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "British Columbia",
				Abbreviation = "BC",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Manitoba",
				Abbreviation = "MB",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "New Brunswick",
				Abbreviation = "NB",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Newfoundland and Labrador",
				Abbreviation = "NL",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Northwest Territories",
				Abbreviation = "NT",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Nova Scotia",
				Abbreviation = "NS",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Nunavut",
				Abbreviation = "NU",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Ontario",
				Abbreviation = "ON",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Prince Edward Island",
				Abbreviation = "PE",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Quebec",
				Abbreviation = "QC",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Saskatchewan",
				Abbreviation = "SK",
				Published = true,
				DisplayOrder = 1,
			});
			cCanada.StateProvinces.Add(new StateProvince()
			{
				Name = "Yukon Territory",
				Abbreviation = "YU",
				Published = true,
				DisplayOrder = 1,
			});
			var countries = new List<Country>
								{
									cUsa,
									cCanada,
									//other countries
									new Country
									{
										Name = "Argentina",
										TwoLetterIsoCode = "AR",
										ThreeLetterIsoCode = "ARG",
										NumericIsoCode = 32,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Armenia",
										TwoLetterIsoCode = "AM",
										ThreeLetterIsoCode = "ARM",
										NumericIsoCode = 51,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Aruba",
										TwoLetterIsoCode = "AW",
										ThreeLetterIsoCode = "ABW",
										NumericIsoCode = 533,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Australia",
										TwoLetterIsoCode = "AU",
										ThreeLetterIsoCode = "AUS",
										NumericIsoCode = 36,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Austria",
										TwoLetterIsoCode = "AT",
										ThreeLetterIsoCode = "AUT",
										NumericIsoCode = 40,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Azerbaijan",
										TwoLetterIsoCode = "AZ",
										ThreeLetterIsoCode = "AZE",
										NumericIsoCode = 31,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bahamas",
										TwoLetterIsoCode = "BS",
										ThreeLetterIsoCode = "BHS",
										NumericIsoCode = 44,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bangladesh",
										TwoLetterIsoCode = "BD",
										ThreeLetterIsoCode = "BGD",
										NumericIsoCode = 50,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Belarus",
										TwoLetterIsoCode = "BY",
										ThreeLetterIsoCode = "BLR",
										NumericIsoCode = 112,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Belgium",
										TwoLetterIsoCode = "BE",
										ThreeLetterIsoCode = "BEL",
										NumericIsoCode = 56,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Belize",
										TwoLetterIsoCode = "BZ",
										ThreeLetterIsoCode = "BLZ",
										NumericIsoCode = 84,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bermuda",
										TwoLetterIsoCode = "BM",
										ThreeLetterIsoCode = "BMU",
										NumericIsoCode = 60,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bolivia",
										TwoLetterIsoCode = "BO",
										ThreeLetterIsoCode = "BOL",
										NumericIsoCode = 68,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bosnia and Herzegowina",
										TwoLetterIsoCode = "BA",
										ThreeLetterIsoCode = "BIH",
										NumericIsoCode = 70,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Brazil",
										TwoLetterIsoCode = "BR",
										ThreeLetterIsoCode = "BRA",
										NumericIsoCode = 76,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bulgaria",
										TwoLetterIsoCode = "BG",
										ThreeLetterIsoCode = "BGR",
										NumericIsoCode = 100,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cayman Islands",
										TwoLetterIsoCode = "KY",
										ThreeLetterIsoCode = "CYM",
										NumericIsoCode = 136,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Chile",
										TwoLetterIsoCode = "CL",
										ThreeLetterIsoCode = "CHL",
										NumericIsoCode = 152,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "China",
										TwoLetterIsoCode = "CN",
										ThreeLetterIsoCode = "CHN",
										NumericIsoCode = 156,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Colombia",
										TwoLetterIsoCode = "CO",
										ThreeLetterIsoCode = "COL",
										NumericIsoCode = 170,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Costa Rica",
										TwoLetterIsoCode = "CR",
										ThreeLetterIsoCode = "CRI",
										NumericIsoCode = 188,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Croatia (local Name: Hrvatska)",
										TwoLetterIsoCode = "HR",
										ThreeLetterIsoCode = "HRV",
										NumericIsoCode = 191,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cuba",
										TwoLetterIsoCode = "CU",
										ThreeLetterIsoCode = "CUB",
										NumericIsoCode = 192,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cyprus",
										TwoLetterIsoCode = "CY",
										ThreeLetterIsoCode = "CYP",
										NumericIsoCode = 196,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Czech Republic",
										TwoLetterIsoCode = "CZ",
										ThreeLetterIsoCode = "CZE",
										NumericIsoCode = 203,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Denmark",
										TwoLetterIsoCode = "DK",
										ThreeLetterIsoCode = "DNK",
										NumericIsoCode = 208,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Dominican Republic",
										TwoLetterIsoCode = "DO",
										ThreeLetterIsoCode = "DOM",
										NumericIsoCode = 214,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Ecuador",
										TwoLetterIsoCode = "EC",
										ThreeLetterIsoCode = "ECU",
										NumericIsoCode = 218,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Egypt",
										TwoLetterIsoCode = "EG",
										ThreeLetterIsoCode = "EGY",
										NumericIsoCode = 818,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Finland",
										TwoLetterIsoCode = "FI",
										ThreeLetterIsoCode = "FIN",
										NumericIsoCode = 246,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "France",
										TwoLetterIsoCode = "FR",
										ThreeLetterIsoCode = "FRA",
										NumericIsoCode = 250,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Georgia",
										TwoLetterIsoCode = "GE",
										ThreeLetterIsoCode = "GEO",
										NumericIsoCode = 268,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Germany",
										TwoLetterIsoCode = "DE",
										ThreeLetterIsoCode = "DEU",
										NumericIsoCode = 276,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Gibraltar",
										TwoLetterIsoCode = "GI",
										ThreeLetterIsoCode = "GIB",
										NumericIsoCode = 292,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Greece",
										TwoLetterIsoCode = "GR",
										ThreeLetterIsoCode = "GRC",
										NumericIsoCode = 300,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Guatemala",
										TwoLetterIsoCode = "GT",
										ThreeLetterIsoCode = "GTM",
										NumericIsoCode = 320,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Hong Kong",
										TwoLetterIsoCode = "HK",
										ThreeLetterIsoCode = "HKG",
										NumericIsoCode = 344,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Hungary",
										TwoLetterIsoCode = "HU",
										ThreeLetterIsoCode = "HUN",
										NumericIsoCode = 348,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "India",
										TwoLetterIsoCode = "IN",
										ThreeLetterIsoCode = "IND",
										NumericIsoCode = 356,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Indonesia",
										TwoLetterIsoCode = "ID",
										ThreeLetterIsoCode = "IDN",
										NumericIsoCode = 360,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Ireland",
										TwoLetterIsoCode = "IE",
										ThreeLetterIsoCode = "IRL",
										NumericIsoCode = 372,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Israel",
										TwoLetterIsoCode = "IL",
										ThreeLetterIsoCode = "ISR",
										NumericIsoCode = 376,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Italy",
										TwoLetterIsoCode = "IT",
										ThreeLetterIsoCode = "ITA",
										NumericIsoCode = 380,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Jamaica",
										TwoLetterIsoCode = "JM",
										ThreeLetterIsoCode = "JAM",
										NumericIsoCode = 388,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Japan",
										TwoLetterIsoCode = "JP",
										ThreeLetterIsoCode = "JPN",
										NumericIsoCode = 392,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Jordan",
										TwoLetterIsoCode = "JO",
										ThreeLetterIsoCode = "JOR",
										NumericIsoCode = 400,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Kazakhstan",
										TwoLetterIsoCode = "KZ",
										ThreeLetterIsoCode = "KAZ",
										NumericIsoCode = 398,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Korea, Democratic People's Republic of",
										TwoLetterIsoCode = "KP",
										ThreeLetterIsoCode = "PRK",
										NumericIsoCode = 408,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Kuwait",
										TwoLetterIsoCode = "KW",
										ThreeLetterIsoCode = "KWT",
										NumericIsoCode = 414,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Malaysia",
										TwoLetterIsoCode = "MY",
										ThreeLetterIsoCode = "MYS",
										NumericIsoCode = 458,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mexico",
										TwoLetterIsoCode = "MX",
										ThreeLetterIsoCode = "MEX",
										NumericIsoCode = 484,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Netherlands",
										TwoLetterIsoCode = "NL",
										ThreeLetterIsoCode = "NLD",
										NumericIsoCode = 528,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "New Zealand",
										TwoLetterIsoCode = "NZ",
										ThreeLetterIsoCode = "NZL",
										NumericIsoCode = 554,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Norway",
										TwoLetterIsoCode = "NO",
										ThreeLetterIsoCode = "NOR",
										NumericIsoCode = 578,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Pakistan",
										TwoLetterIsoCode = "PK",
										ThreeLetterIsoCode = "PAK",
										NumericIsoCode = 586,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Paraguay",
										TwoLetterIsoCode = "PY",
										ThreeLetterIsoCode = "PRY",
										NumericIsoCode = 600,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Peru",
										TwoLetterIsoCode = "PE",
										ThreeLetterIsoCode = "PER",
										NumericIsoCode = 604,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Philippines",
										TwoLetterIsoCode = "PH",
										ThreeLetterIsoCode = "PHL",
										NumericIsoCode = 608,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Poland",
										TwoLetterIsoCode = "PL",
										ThreeLetterIsoCode = "POL",
										NumericIsoCode = 616,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Portugal",
										TwoLetterIsoCode = "PT",
										ThreeLetterIsoCode = "PRT",
										NumericIsoCode = 620,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Puerto Rico",
										TwoLetterIsoCode = "PR",
										ThreeLetterIsoCode = "PRI",
										NumericIsoCode = 630,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Qatar",
										TwoLetterIsoCode = "QA",
										ThreeLetterIsoCode = "QAT",
										NumericIsoCode = 634,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Romania",
										TwoLetterIsoCode = "RO",
										ThreeLetterIsoCode = "ROM",
										NumericIsoCode = 642,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Russia",
										TwoLetterIsoCode = "RU",
										ThreeLetterIsoCode = "RUS",
										NumericIsoCode = 643,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Saudi Arabia",
										TwoLetterIsoCode = "SA",
										ThreeLetterIsoCode = "SAU",
										NumericIsoCode = 682,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Singapore",
										TwoLetterIsoCode = "SG",
										ThreeLetterIsoCode = "SGP",
										NumericIsoCode = 702,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Slovakia (Slovak Republic)",
										TwoLetterIsoCode = "SK",
										ThreeLetterIsoCode = "SVK",
										NumericIsoCode = 703,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Slovenia",
										TwoLetterIsoCode = "SI",
										ThreeLetterIsoCode = "SVN",
										NumericIsoCode = 705,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "South Africa",
										TwoLetterIsoCode = "ZA",
										ThreeLetterIsoCode = "ZAF",
										NumericIsoCode = 710,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Spain",
										TwoLetterIsoCode = "ES",
										ThreeLetterIsoCode = "ESP",
										NumericIsoCode = 724,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Sweden",
										TwoLetterIsoCode = "SE",
										ThreeLetterIsoCode = "SWE",
										NumericIsoCode = 752,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Switzerland",
										TwoLetterIsoCode = "CH",
										ThreeLetterIsoCode = "CHE",
										NumericIsoCode = 756,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Taiwan",
										TwoLetterIsoCode = "TW",
										ThreeLetterIsoCode = "TWN",
										NumericIsoCode = 158,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Thailand",
										TwoLetterIsoCode = "TH",
										ThreeLetterIsoCode = "THA",
										NumericIsoCode = 764,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Turkey",
										TwoLetterIsoCode = "TR",
										ThreeLetterIsoCode = "TUR",
										NumericIsoCode = 792,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Ukraine",
										TwoLetterIsoCode = "UA",
										ThreeLetterIsoCode = "UKR",
										NumericIsoCode = 804,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "United Arab Emirates",
										TwoLetterIsoCode = "AE",
										ThreeLetterIsoCode = "ARE",
										NumericIsoCode = 784,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "United Kingdom",
										TwoLetterIsoCode = "GB",
										ThreeLetterIsoCode = "GBR",
										NumericIsoCode = 826,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "United States minor outlying islands",
										TwoLetterIsoCode = "UM",
										ThreeLetterIsoCode = "UMI",
										NumericIsoCode = 581,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Uruguay",
										TwoLetterIsoCode = "UY",
										ThreeLetterIsoCode = "URY",
										NumericIsoCode = 858,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Uzbekistan",
										TwoLetterIsoCode = "UZ",
										ThreeLetterIsoCode = "UZB",
										NumericIsoCode = 860,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Venezuela",
										TwoLetterIsoCode = "VE",
										ThreeLetterIsoCode = "VEN",
										NumericIsoCode = 862,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Serbia",
										TwoLetterIsoCode = "RS",
										ThreeLetterIsoCode = "SRB",
										NumericIsoCode = 688,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Afghanistan",
										TwoLetterIsoCode = "AF",
										ThreeLetterIsoCode = "AFG",
										NumericIsoCode = 4,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Albania",
										TwoLetterIsoCode = "AL",
										ThreeLetterIsoCode = "ALB",
										NumericIsoCode = 8,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Algeria",
										TwoLetterIsoCode = "DZ",
										ThreeLetterIsoCode = "DZA",
										NumericIsoCode = 12,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "American Samoa",
										TwoLetterIsoCode = "AS",
										ThreeLetterIsoCode = "ASM",
										NumericIsoCode = 16,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Andorra",
										TwoLetterIsoCode = "AD",
										ThreeLetterIsoCode = "AND",
										NumericIsoCode = 20,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Angola",
										TwoLetterIsoCode = "AO",
										ThreeLetterIsoCode = "AGO",
										NumericIsoCode = 24,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Anguilla",
										TwoLetterIsoCode = "AI",
										ThreeLetterIsoCode = "AIA",
										NumericIsoCode = 660,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Antarctica",
										TwoLetterIsoCode = "AQ",
										ThreeLetterIsoCode = "ATA",
										NumericIsoCode = 10,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Antigua and Barbuda",
										TwoLetterIsoCode = "AG",
										ThreeLetterIsoCode = "ATG",
										NumericIsoCode = 28,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bahrain",
										TwoLetterIsoCode = "BH",
										ThreeLetterIsoCode = "BHR",
										NumericIsoCode = 48,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Barbados",
										TwoLetterIsoCode = "BB",
										ThreeLetterIsoCode = "BRB",
										NumericIsoCode = 52,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Benin",
										TwoLetterIsoCode = "BJ",
										ThreeLetterIsoCode = "BEN",
										NumericIsoCode = 204,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bhutan",
										TwoLetterIsoCode = "BT",
										ThreeLetterIsoCode = "BTN",
										NumericIsoCode = 64,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Botswana",
										TwoLetterIsoCode = "BW",
										ThreeLetterIsoCode = "BWA",
										NumericIsoCode = 72,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Bouvet Island",
										TwoLetterIsoCode = "BV",
										ThreeLetterIsoCode = "BVT",
										NumericIsoCode = 74,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "British Indian Ocean Territory",
										TwoLetterIsoCode = "IO",
										ThreeLetterIsoCode = "IOT",
										NumericIsoCode = 86,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Brunei Darussalam",
										TwoLetterIsoCode = "BN",
										ThreeLetterIsoCode = "BRN",
										NumericIsoCode = 96,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Burkina Faso",
										TwoLetterIsoCode = "BF",
										ThreeLetterIsoCode = "BFA",
										NumericIsoCode = 854,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Burundi",
										TwoLetterIsoCode = "BI",
										ThreeLetterIsoCode = "BDI",
										NumericIsoCode = 108,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cambodia",
										TwoLetterIsoCode = "KH",
										ThreeLetterIsoCode = "KHM",
										NumericIsoCode = 116,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cameroon",
										TwoLetterIsoCode = "CM",
										ThreeLetterIsoCode = "CMR",
										NumericIsoCode = 120,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cape Verde",
										TwoLetterIsoCode = "CV",
										ThreeLetterIsoCode = "CPV",
										NumericIsoCode = 132,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Central African Republic",
										TwoLetterIsoCode = "CF",
										ThreeLetterIsoCode = "CAF",
										NumericIsoCode = 140,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Chad",
										TwoLetterIsoCode = "TD",
										ThreeLetterIsoCode = "TCD",
										NumericIsoCode = 148,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Christmas Island",
										TwoLetterIsoCode = "CX",
										ThreeLetterIsoCode = "CXR",
										NumericIsoCode = 162,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cocos (Keeling) Islands",
										TwoLetterIsoCode = "CC",
										ThreeLetterIsoCode = "CCK",
										NumericIsoCode = 166,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Comoros",
										TwoLetterIsoCode = "KM",
										ThreeLetterIsoCode = "COM",
										NumericIsoCode = 174,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Congo",
										TwoLetterIsoCode = "CG",
										ThreeLetterIsoCode = "COG",
										NumericIsoCode = 178,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cook Islands",
										TwoLetterIsoCode = "CK",
										ThreeLetterIsoCode = "COK",
										NumericIsoCode = 184,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Cote D'Ivoire",
										TwoLetterIsoCode = "CI",
										ThreeLetterIsoCode = "CIV",
										NumericIsoCode = 384,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Djibouti",
										TwoLetterIsoCode = "DJ",
										ThreeLetterIsoCode = "DJI",
										NumericIsoCode = 262,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Dominica",
										TwoLetterIsoCode = "DM",
										ThreeLetterIsoCode = "DMA",
										NumericIsoCode = 212,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "El Salvador",
										TwoLetterIsoCode = "SV",
										ThreeLetterIsoCode = "SLV",
										NumericIsoCode = 222,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Equatorial Guinea",
										TwoLetterIsoCode = "GQ",
										ThreeLetterIsoCode = "GNQ",
										NumericIsoCode = 226,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Eritrea",
										TwoLetterIsoCode = "ER",
										ThreeLetterIsoCode = "ERI",
										NumericIsoCode = 232,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Estonia",
										TwoLetterIsoCode = "EE",
										ThreeLetterIsoCode = "EST",
										NumericIsoCode = 233,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Ethiopia",
										TwoLetterIsoCode = "ET",
										ThreeLetterIsoCode = "ETH",
										NumericIsoCode = 231,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Falkland Islands (Malvinas)",
										TwoLetterIsoCode = "FK",
										ThreeLetterIsoCode = "FLK",
										NumericIsoCode = 238,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Faroe Islands",
										TwoLetterIsoCode = "FO",
										ThreeLetterIsoCode = "FRO",
										NumericIsoCode = 234,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Fiji",
										TwoLetterIsoCode = "FJ",
										ThreeLetterIsoCode = "FJI",
										NumericIsoCode = 242,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "French Guiana",
										TwoLetterIsoCode = "GF",
										ThreeLetterIsoCode = "GUF",
										NumericIsoCode = 254,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "French Polynesia",
										TwoLetterIsoCode = "PF",
										ThreeLetterIsoCode = "PYF",
										NumericIsoCode = 258,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "French Southern Territories",
										TwoLetterIsoCode = "TF",
										ThreeLetterIsoCode = "ATF",
										NumericIsoCode = 260,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Gabon",
										TwoLetterIsoCode = "GA",
										ThreeLetterIsoCode = "GAB",
										NumericIsoCode = 266,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Gambia",
										TwoLetterIsoCode = "GM",
										ThreeLetterIsoCode = "GMB",
										NumericIsoCode = 270,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Ghana",
										TwoLetterIsoCode = "GH",
										ThreeLetterIsoCode = "GHA",
										NumericIsoCode = 288,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Greenland",
										TwoLetterIsoCode = "GL",
										ThreeLetterIsoCode = "GRL",
										NumericIsoCode = 304,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Grenada",
										TwoLetterIsoCode = "GD",
										ThreeLetterIsoCode = "GRD",
										NumericIsoCode = 308,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Guadeloupe",
										TwoLetterIsoCode = "GP",
										ThreeLetterIsoCode = "GLP",
										NumericIsoCode = 312,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Guam",
										TwoLetterIsoCode = "GU",
										ThreeLetterIsoCode = "GUM",
										NumericIsoCode = 316,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Guinea",
										TwoLetterIsoCode = "GN",
										ThreeLetterIsoCode = "GIN",
										NumericIsoCode = 324,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Guinea-bissau",
										TwoLetterIsoCode = "GW",
										ThreeLetterIsoCode = "GNB",
										NumericIsoCode = 624,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Guyana",
										TwoLetterIsoCode = "GY",
										ThreeLetterIsoCode = "GUY",
										NumericIsoCode = 328,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Haiti",
										TwoLetterIsoCode = "HT",
										ThreeLetterIsoCode = "HTI",
										NumericIsoCode = 332,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Heard and Mc Donald Islands",
										TwoLetterIsoCode = "HM",
										ThreeLetterIsoCode = "HMD",
										NumericIsoCode = 334,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Honduras",
										TwoLetterIsoCode = "HN",
										ThreeLetterIsoCode = "HND",
										NumericIsoCode = 340,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Iceland",
										TwoLetterIsoCode = "IS",
										ThreeLetterIsoCode = "ISL",
										NumericIsoCode = 352,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Iran (Islamic Republic of)",
										TwoLetterIsoCode = "IR",
										ThreeLetterIsoCode = "IRN",
										NumericIsoCode = 364,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Iraq",
										TwoLetterIsoCode = "IQ",
										ThreeLetterIsoCode = "IRQ",
										NumericIsoCode = 368,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Kenya",
										TwoLetterIsoCode = "KE",
										ThreeLetterIsoCode = "KEN",
										NumericIsoCode = 404,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Kiribati",
										TwoLetterIsoCode = "KI",
										ThreeLetterIsoCode = "KIR",
										NumericIsoCode = 296,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Korea",
										TwoLetterIsoCode = "KR",
										ThreeLetterIsoCode = "KOR",
										NumericIsoCode = 410,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Kyrgyzstan",
										TwoLetterIsoCode = "KG",
										ThreeLetterIsoCode = "KGZ",
										NumericIsoCode = 417,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Lao People's Democratic Republic",
										TwoLetterIsoCode = "LA",
										ThreeLetterIsoCode = "LAO",
										NumericIsoCode = 418,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Latvia",
										TwoLetterIsoCode = "LV",
										ThreeLetterIsoCode = "LVA",
										NumericIsoCode = 428,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Lebanon",
										TwoLetterIsoCode = "LB",
										ThreeLetterIsoCode = "LBN",
										NumericIsoCode = 422,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Lesotho",
										TwoLetterIsoCode = "LS",
										ThreeLetterIsoCode = "LSO",
										NumericIsoCode = 426,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Liberia",
										TwoLetterIsoCode = "LR",
										ThreeLetterIsoCode = "LBR",
										NumericIsoCode = 430,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Libyan Arab Jamahiriya",
										TwoLetterIsoCode = "LY",
										ThreeLetterIsoCode = "LBY",
										NumericIsoCode = 434,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Liechtenstein",
										TwoLetterIsoCode = "LI",
										ThreeLetterIsoCode = "LIE",
										NumericIsoCode = 438,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Lithuania",
										TwoLetterIsoCode = "LT",
										ThreeLetterIsoCode = "LTU",
										NumericIsoCode = 440,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Luxembourg",
										TwoLetterIsoCode = "LU",
										ThreeLetterIsoCode = "LUX",
										NumericIsoCode = 442,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Macau",
										TwoLetterIsoCode = "MO",
										ThreeLetterIsoCode = "MAC",
										NumericIsoCode = 446,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Macedonia",
										TwoLetterIsoCode = "MK",
										ThreeLetterIsoCode = "MKD",
										NumericIsoCode = 807,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Madagascar",
										TwoLetterIsoCode = "MG",
										ThreeLetterIsoCode = "MDG",
										NumericIsoCode = 450,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Malawi",
										TwoLetterIsoCode = "MW",
										ThreeLetterIsoCode = "MWI",
										NumericIsoCode = 454,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Maldives",
										TwoLetterIsoCode = "MV",
										ThreeLetterIsoCode = "MDV",
										NumericIsoCode = 462,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mali",
										TwoLetterIsoCode = "ML",
										ThreeLetterIsoCode = "MLI",
										NumericIsoCode = 466,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Malta",
										TwoLetterIsoCode = "MT",
										ThreeLetterIsoCode = "MLT",
										NumericIsoCode = 470,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Marshall Islands",
										TwoLetterIsoCode = "MH",
										ThreeLetterIsoCode = "MHL",
										NumericIsoCode = 584,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Martinique",
										TwoLetterIsoCode = "MQ",
										ThreeLetterIsoCode = "MTQ",
										NumericIsoCode = 474,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mauritania",
										TwoLetterIsoCode = "MR",
										ThreeLetterIsoCode = "MRT",
										NumericIsoCode = 478,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mauritius",
										TwoLetterIsoCode = "MU",
										ThreeLetterIsoCode = "MUS",
										NumericIsoCode = 480,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mayotte",
										TwoLetterIsoCode = "YT",
										ThreeLetterIsoCode = "MYT",
										NumericIsoCode = 175,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Micronesia",
										TwoLetterIsoCode = "FM",
										ThreeLetterIsoCode = "FSM",
										NumericIsoCode = 583,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Moldova",
										TwoLetterIsoCode = "MD",
										ThreeLetterIsoCode = "MDA",
										NumericIsoCode = 498,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Monaco",
										TwoLetterIsoCode = "MC",
										ThreeLetterIsoCode = "MCO",
										NumericIsoCode = 492,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mongolia",
										TwoLetterIsoCode = "MN",
										ThreeLetterIsoCode = "MNG",
										NumericIsoCode = 496,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Montserrat",
										TwoLetterIsoCode = "MS",
										ThreeLetterIsoCode = "MSR",
										NumericIsoCode = 500,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Morocco",
										TwoLetterIsoCode = "MA",
										ThreeLetterIsoCode = "MAR",
										NumericIsoCode = 504,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Mozambique",
										TwoLetterIsoCode = "MZ",
										ThreeLetterIsoCode = "MOZ",
										NumericIsoCode = 508,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Myanmar",
										TwoLetterIsoCode = "MM",
										ThreeLetterIsoCode = "MMR",
										NumericIsoCode = 104,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Namibia",
										TwoLetterIsoCode = "NA",
										ThreeLetterIsoCode = "NAM",
										NumericIsoCode = 516,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Nauru",
										TwoLetterIsoCode = "NR",
										ThreeLetterIsoCode = "NRU",
										NumericIsoCode = 520,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Nepal",
										TwoLetterIsoCode = "NP",
										ThreeLetterIsoCode = "NPL",
										NumericIsoCode = 524,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Netherlands Antilles",
										TwoLetterIsoCode = "AN",
										ThreeLetterIsoCode = "ANT",
										NumericIsoCode = 530,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "New Caledonia",
										TwoLetterIsoCode = "NC",
										ThreeLetterIsoCode = "NCL",
										NumericIsoCode = 540,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Nicaragua",
										TwoLetterIsoCode = "NI",
										ThreeLetterIsoCode = "NIC",
										NumericIsoCode = 558,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Niger",
										TwoLetterIsoCode = "NE",
										ThreeLetterIsoCode = "NER",
										NumericIsoCode = 562,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Nigeria",
										TwoLetterIsoCode = "NG",
										ThreeLetterIsoCode = "NGA",
										NumericIsoCode = 566,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Niue",
										TwoLetterIsoCode = "NU",
										ThreeLetterIsoCode = "NIU",
										NumericIsoCode = 570,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Norfolk Island",
										TwoLetterIsoCode = "NF",
										ThreeLetterIsoCode = "NFK",
										NumericIsoCode = 574,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Northern Mariana Islands",
										TwoLetterIsoCode = "MP",
										ThreeLetterIsoCode = "MNP",
										NumericIsoCode = 580,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Oman",
										TwoLetterIsoCode = "OM",
										ThreeLetterIsoCode = "OMN",
										NumericIsoCode = 512,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Palau",
										TwoLetterIsoCode = "PW",
										ThreeLetterIsoCode = "PLW",
										NumericIsoCode = 585,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Panama",
										TwoLetterIsoCode = "PA",
										ThreeLetterIsoCode = "PAN",
										NumericIsoCode = 591,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Papua New Guinea",
										TwoLetterIsoCode = "PG",
										ThreeLetterIsoCode = "PNG",
										NumericIsoCode = 598,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Pitcairn",
										TwoLetterIsoCode = "PN",
										ThreeLetterIsoCode = "PCN",
										NumericIsoCode = 612,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Reunion",
										TwoLetterIsoCode = "RE",
										ThreeLetterIsoCode = "REU",
										NumericIsoCode = 638,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Rwanda",
										TwoLetterIsoCode = "RW",
										ThreeLetterIsoCode = "RWA",
										NumericIsoCode = 646,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Saint Kitts and Nevis",
										TwoLetterIsoCode = "KN",
										ThreeLetterIsoCode = "KNA",
										NumericIsoCode = 659,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Saint Lucia",
										TwoLetterIsoCode = "LC",
										ThreeLetterIsoCode = "LCA",
										NumericIsoCode = 662,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Saint Vincent and the Grenadines",
										TwoLetterIsoCode = "VC",
										ThreeLetterIsoCode = "VCT",
										NumericIsoCode = 670,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Samoa",
										TwoLetterIsoCode = "WS",
										ThreeLetterIsoCode = "WSM",
										NumericIsoCode = 882,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "San Marino",
										TwoLetterIsoCode = "SM",
										ThreeLetterIsoCode = "SMR",
										NumericIsoCode = 674,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Sao Tome and Principe",
										TwoLetterIsoCode = "ST",
										ThreeLetterIsoCode = "STP",
										NumericIsoCode = 678,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Senegal",
										TwoLetterIsoCode = "SN",
										ThreeLetterIsoCode = "SEN",
										NumericIsoCode = 686,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Seychelles",
										TwoLetterIsoCode = "SC",
										ThreeLetterIsoCode = "SYC",
										NumericIsoCode = 690,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Sierra Leone",
										TwoLetterIsoCode = "SL",
										ThreeLetterIsoCode = "SLE",
										NumericIsoCode = 694,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Solomon Islands",
										TwoLetterIsoCode = "SB",
										ThreeLetterIsoCode = "SLB",
										NumericIsoCode = 90,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Somalia",
										TwoLetterIsoCode = "SO",
										ThreeLetterIsoCode = "SOM",
										NumericIsoCode = 706,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "South Georgia & South Sandwich Islands",
										TwoLetterIsoCode = "GS",
										ThreeLetterIsoCode = "SGS",
										NumericIsoCode = 239,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Sri Lanka",
										TwoLetterIsoCode = "LK",
										ThreeLetterIsoCode = "LKA",
										NumericIsoCode = 144,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "St. Helena",
										TwoLetterIsoCode = "SH",
										ThreeLetterIsoCode = "SHN",
										NumericIsoCode = 654,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "St. Pierre and Miquelon",
										TwoLetterIsoCode = "PM",
										ThreeLetterIsoCode = "SPM",
										NumericIsoCode = 666,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Sudan",
										TwoLetterIsoCode = "SD",
										ThreeLetterIsoCode = "SDN",
										NumericIsoCode = 736,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Suriname",
										TwoLetterIsoCode = "SR",
										ThreeLetterIsoCode = "SUR",
										NumericIsoCode = 740,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Svalbard and Jan Mayen Islands",
										TwoLetterIsoCode = "SJ",
										ThreeLetterIsoCode = "SJM",
										NumericIsoCode = 744,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Swaziland",
										TwoLetterIsoCode = "SZ",
										ThreeLetterIsoCode = "SWZ",
										NumericIsoCode = 748,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Syrian Arab Republic",
										TwoLetterIsoCode = "SY",
										ThreeLetterIsoCode = "SYR",
										NumericIsoCode = 760,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Tajikistan",
										TwoLetterIsoCode = "TJ",
										ThreeLetterIsoCode = "TJK",
										NumericIsoCode = 762,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Tanzania",
										TwoLetterIsoCode = "TZ",
										ThreeLetterIsoCode = "TZA",
										NumericIsoCode = 834,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Togo",
										TwoLetterIsoCode = "TG",
										ThreeLetterIsoCode = "TGO",
										NumericIsoCode = 768,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Tokelau",
										TwoLetterIsoCode = "TK",
										ThreeLetterIsoCode = "TKL",
										NumericIsoCode = 772,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Tonga",
										TwoLetterIsoCode = "TO",
										ThreeLetterIsoCode = "TON",
										NumericIsoCode = 776,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Trinidad and Tobago",
										TwoLetterIsoCode = "TT",
										ThreeLetterIsoCode = "TTO",
										NumericIsoCode = 780,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Tunisia",
										TwoLetterIsoCode = "TN",
										ThreeLetterIsoCode = "TUN",
										NumericIsoCode = 788,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Turkmenistan",
										TwoLetterIsoCode = "TM",
										ThreeLetterIsoCode = "TKM",
										NumericIsoCode = 795,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Turks and Caicos Islands",
										TwoLetterIsoCode = "TC",
										ThreeLetterIsoCode = "TCA",
										NumericIsoCode = 796,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Tuvalu",
										TwoLetterIsoCode = "TV",
										ThreeLetterIsoCode = "TUV",
										NumericIsoCode = 798,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Uganda",
										TwoLetterIsoCode = "UG",
										ThreeLetterIsoCode = "UGA",
										NumericIsoCode = 800,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Vanuatu",
										TwoLetterIsoCode = "VU",
										ThreeLetterIsoCode = "VUT",
										NumericIsoCode = 548,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Vatican City State (Holy See)",
										TwoLetterIsoCode = "VA",
										ThreeLetterIsoCode = "VAT",
										NumericIsoCode = 336,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Viet Nam",
										TwoLetterIsoCode = "VN",
										ThreeLetterIsoCode = "VNM",
										NumericIsoCode = 704,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Virgin Islands (British)",
										TwoLetterIsoCode = "VG",
										ThreeLetterIsoCode = "VGB",
										NumericIsoCode = 92,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Virgin Islands (U.S.)",
										TwoLetterIsoCode = "VI",
										ThreeLetterIsoCode = "VIR",
										NumericIsoCode = 850,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Wallis and Futuna Islands",
										TwoLetterIsoCode = "WF",
										ThreeLetterIsoCode = "WLF",
										NumericIsoCode = 876,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Western Sahara",
										TwoLetterIsoCode = "EH",
										ThreeLetterIsoCode = "ESH",
										NumericIsoCode = 732,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Yemen",
										TwoLetterIsoCode = "YE",
										ThreeLetterIsoCode = "YEM",
										NumericIsoCode = 887,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Zambia",
										TwoLetterIsoCode = "ZM",
										ThreeLetterIsoCode = "ZMB",
										NumericIsoCode = 894,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Zimbabwe",
										TwoLetterIsoCode = "ZW",
										ThreeLetterIsoCode = "ZWE",
										NumericIsoCode = 716,
										DisplayOrder = 100,
										Published = true
									},
								};
			countries.ForEach(c => _countryRepository.Insert(c));
		}

		protected virtual void InstallUsersAndUsers(string defaultUserEmail, string defaultUserPassword)
		{
			var crAdministrators = new UserRole
			{
				Name = "Administrators",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemUserRoleNames.Administrators,
			};
			var crForumModerators = new UserRole
			{
				Name = "Forum Moderators",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemUserRoleNames.ForumModerators,
			};
			var crRegistered = new UserRole
			{
				Name = "Registered",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemUserRoleNames.Registered,
			};
			var crGuests = new UserRole
			{
				Name = "Guests",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemUserRoleNames.Guests,
			};
            var crBuyer = new UserRole
            {
                Name = "Buyer",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemUserRoleNames.Buyer,
            };
            var crRequestor = new UserRole
            {
                Name = "Requestor",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemUserRoleNames.Requestor,
            };
			var userRoles = new List<UserRole>
								{
									crAdministrators,
									crForumModerators,
									crRegistered,
									crGuests,
                                    crBuyer,
                                    crRequestor
								};
			userRoles.ForEach(cr => _userRoleRepository.Insert(cr));

			//admin user
			var adminUser = new User()
			{
				UserGuid = Guid.NewGuid(),
				Email = defaultUserEmail,
				Username = defaultUserEmail,
				Password = defaultUserPassword,
				PasswordFormat = PasswordFormat.Clear,
				PasswordSalt = "",
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc= DateTime.UtcNow
			};
			var defaultAdminUserAddress = new Address()
			{
				FirstName = "Antonio Amiel",
				LastName = "Yap",
				PhoneNumber = "12345678",
				Email = "antonio-amiel.yap@teradyne.com",
				FaxNumber = "",
				Company = "Teradyne Phils. Ltd.",
				Address1 = "Masskara St. Basak",
				Address2 = "",
				City = "Lapu-lapu City",
				StateProvince = _stateProvinceRepository.Table.Where(sp => sp.Name == "").FirstOrDefault(),
				Country = _countryRepository.Table.Where(c => c.ThreeLetterIsoCode == "PHL").FirstOrDefault(),
				ZipPostalCode = "6015",
				CreatedOnUtc = DateTime.UtcNow,
                
			};
			adminUser.Addresses.Add(defaultAdminUserAddress);
			adminUser.UserRoles.Add(crAdministrators);
			adminUser.UserRoles.Add(crForumModerators);
			adminUser.UserRoles.Add(crRegistered);
			_userRepository.Insert(adminUser);
			//set default user name
            _genericAttributeService.SaveAttribute(adminUser, SystemUserAttributeNames.FirstName, defaultAdminUserAddress.FirstName);
            _genericAttributeService.SaveAttribute(adminUser, SystemUserAttributeNames.LastName, defaultAdminUserAddress.LastName);

            // Buyers
            var dianneBuyer = new User()
			    {
				    UserGuid = Guid.NewGuid(),
				    Email = "dianne-marie.dela-cruz@teradyne.com",
				    Username = "delacru",
				    Password = "delacru",
				    PasswordFormat = PasswordFormat.Clear,
				    PasswordSalt = "",
				    Active = true,
				    CreatedOnUtc = DateTime.UtcNow,
				    LastActivityDateUtc= DateTime.UtcNow
			    };

            var dianneAddress = new Address()
            {
                FirstName = "Dianne-Marie",
                LastName = "Dela-Cruz",
                PhoneNumber = "12345678",
                Email = "dianne-marie.dela-cruz@teradyne.com",
                FaxNumber = "",
                Company = "Teradyne Phils. Ltd.",
                Address1 = "Masskara St. Basak",
                Address2 = "",
                City = "Lapu-lapu City",
                StateProvince = _stateProvinceRepository.Table.Where(sp => sp.Name == "").FirstOrDefault(),
                Country = _countryRepository.Table.Where(c => c.ThreeLetterIsoCode == "PHL").FirstOrDefault(),
                ZipPostalCode = "6015",
                CreatedOnUtc = DateTime.UtcNow
            };

            dianneBuyer.Addresses.Add(dianneAddress);
            dianneBuyer.UserRoles.Add(crRegistered);
            dianneBuyer.UserRoles.Add(crAdministrators);
            dianneBuyer.UserRoles.Add(crBuyer);
            _userRepository.Insert(dianneBuyer);
            //set default user name
            _genericAttributeService.SaveAttribute(dianneBuyer, SystemUserAttributeNames.FirstName, dianneAddress.FirstName);
            _genericAttributeService.SaveAttribute(dianneBuyer, SystemUserAttributeNames.LastName, dianneAddress.LastName);

            var rultherBuyer = new User()
            {
                UserGuid = Guid.NewGuid(),
                Email = "rulther.boncales@teradyne.com",
                Username = "boncaler",
                Password = "boncaler",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = "",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };

            var rultherAddress = new Address()
            {
                FirstName = "Rulther",
                LastName = "Boncales",
                PhoneNumber = "12345678",
                Email = "rulther.boncales@teradyne.com",
                FaxNumber = "",
                Company = "Teradyne Phils. Ltd.",
                Address1 = "Masskara St. Basak",
                Address2 = "",
                City = "Lapu-lapu City",
                StateProvince = _stateProvinceRepository.Table.Where(sp => sp.Name == "").FirstOrDefault(),
                Country = _countryRepository.Table.Where(c => c.ThreeLetterIsoCode == "PHL").FirstOrDefault(),
                ZipPostalCode = "6015",
                CreatedOnUtc = DateTime.UtcNow
            };

            rultherBuyer.Addresses.Add(rultherAddress);
            rultherBuyer.UserRoles.Add(crRegistered);
            rultherBuyer.UserRoles.Add(crAdministrators);
            rultherBuyer.UserRoles.Add(crBuyer);
            _userRepository.Insert(rultherBuyer);
            //set default user name
            _genericAttributeService.SaveAttribute(rultherBuyer, SystemUserAttributeNames.FirstName, rultherAddress.FirstName);
            _genericAttributeService.SaveAttribute(rultherBuyer, SystemUserAttributeNames.LastName, rultherAddress.LastName);

            var sheilaBuyer = new User()
            {
                UserGuid = Guid.NewGuid(),
                Email = "sheila.tutor@teradyne.com",
                Username = "tutors",
                Password = "tutors",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = "",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };

            var sheilaAddress = new Address()
            {
                FirstName = "Sheila",
                LastName = "Tutor",
                PhoneNumber = "12345678",
                Email = "sheila.tutor@teradyne.com",
                FaxNumber = "",
                Company = "Teradyne Phils. Ltd.",
                Address1 = "Masskara St. Basak",
                Address2 = "",
                City = "Lapu-lapu City",
                StateProvince = _stateProvinceRepository.Table.Where(sp => sp.Name == "").FirstOrDefault(),
                Country = _countryRepository.Table.Where(c => c.ThreeLetterIsoCode == "PHL").FirstOrDefault(),
                ZipPostalCode = "6015",
                CreatedOnUtc = DateTime.UtcNow,
            };

            sheilaBuyer.Addresses.Add(sheilaAddress);
            sheilaBuyer.UserRoles.Add(crRegistered);
            sheilaBuyer.UserRoles.Add(crAdministrators);
            sheilaBuyer.UserRoles.Add(crBuyer);
            _userRepository.Insert(sheilaBuyer);
            //set default user name
            _genericAttributeService.SaveAttribute(sheilaBuyer, SystemUserAttributeNames.FirstName, sheilaAddress.FirstName);
            _genericAttributeService.SaveAttribute(sheilaBuyer, SystemUserAttributeNames.LastName, sheilaAddress.LastName);

            var lloydBuyer = new User()
            {
                UserGuid = Guid.NewGuid(),
                Email = "lloyd.perez@teradyne.com",
                Username = "perezllo",
                Password = "perezllo",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = "",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };

            var lloydAddress = new Address()
            {
                FirstName = "Lloyd",
                LastName = "Perez",
                PhoneNumber = "12345678",
                Email = "lloyd.perez@teradyne.com",
                FaxNumber = "",
                Company = "Teradyne Phils. Ltd.",
                Address1 = "Masskara St. Basak",
                Address2 = "",
                City = "Lapu-lapu City",
                StateProvince = _stateProvinceRepository.Table.Where(sp => sp.Name == "").FirstOrDefault(),
                Country = _countryRepository.Table.Where(c => c.ThreeLetterIsoCode == "PHL").FirstOrDefault(),
                ZipPostalCode = "6015",
                CreatedOnUtc = DateTime.UtcNow,
            };

            lloydBuyer.Addresses.Add(lloydAddress);
            lloydBuyer.UserRoles.Add(crRegistered);
            lloydBuyer.UserRoles.Add(crAdministrators);
            lloydBuyer.UserRoles.Add(crBuyer);
            _userRepository.Insert(lloydBuyer);
            //set default user name
            _genericAttributeService.SaveAttribute(lloydBuyer, SystemUserAttributeNames.FirstName, lloydAddress.FirstName);
            _genericAttributeService.SaveAttribute(lloydBuyer, SystemUserAttributeNames.LastName, lloydAddress.LastName);

			//search engine (crawler) built-in user
			var searchEngineUser = new User()
			{
				Email = "builtin@search_engine_record.com",
				UserGuid = Guid.NewGuid(),
				PasswordFormat = PasswordFormat.Clear,
				AdminComment = "Built-in system guest record used for requests from search engines.",
				Active = true,
				IsSystemAccount = true,
				SystemName = SystemUserNames.SearchEngine,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
			};
			searchEngineUser.UserRoles.Add(crGuests);
			_userRepository.Insert(searchEngineUser);
            
		}

		protected virtual void HashDefaultUserPassword(string defaultUserEmail, string defaultUserPassword)
		{
			var userRegistrationService = EngineContext.Current.Resolve<IUserRegistrationService>();
			userRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, false,
				 PasswordFormat.Hashed, defaultUserPassword));
		}

		protected virtual void InstallEmailAccounts()
		{
			var emailAccounts = new List<EmailAccount>
							   {
								   new EmailAccount
									   {
										   Email = "ssg_cebu@notes.teradyne.com",
										   DisplayName = "SSG Cebu Contact",
										   Host = "131.101.210.107",
										   Port = 25,
										   Username = "",
										   Password = "",
										   EnableSsl = false,
										   UseDefaultCredentials = false
									   },
							   };
			emailAccounts.ForEach(ea => _emailAccountRepository.Insert(ea));

		}

		protected virtual void InstallMessageTemplates()
		{
            var eaGeneral = _emailAccountRepository.Table.Where(ea => ea.DisplayName.Equals("SSG Cebu Contact")).FirstOrDefault();
			//var eaSale = _emailAccountRepository.Table.Where(ea => ea.DisplayName.Equals("Sales representative")).FirstOrDefault();
			//var eaUser = _emailAccountRepository.Table.Where(ea => ea.DisplayName.Equals("User support")).FirstOrDefault();
			var messageTemplates = new List<MessageTemplate>
							   {
								   new MessageTemplate
									   {
										   Name = "User.EmailValidationMessage",
										   Subject = "%Site.Name%. Email validation",
										   Body = "<a href=\"%Site.URL%\">%Site.Name%</a>  <br />  <br />  To activate your account <a href=\"%User.AccountActivationURL%\">click here</a>.     <br />  <br />  %Site.Name%",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "User.NewPM",
										   Subject = "%Site.Name%. You have received a new private message",
										   Body = "<p><a href=\"%Site.URL%\">%Site.Name%</a> <br /><br />You have received a new private message.</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "User.PasswordRecovery",
										   Subject = "%Site.Name%. Password recovery",
										   Body = "<a href=\"%Site.URL%\">%Site.Name%</a>  <br />  <br />  To change your password <a href=\"%User.PasswordRecoveryURL%\">click here</a>.     <br />  <br />  %Site.Name%",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "User.WelcomeMessage",
										   Subject = "Welcome to %Site.Name%",
										   Body = "We welcome you to <a href=\"%Site.URL%\"> %Site.Name%</a>.<br /><br />You can now take part in the various services we have to offer you.<br /><br />For help with any of our online services, please email the site-owner: <a href=\"mailto:%Site.Email%\">%Site.Email%</a>.<br /><br />Note: This email address was given to us by one of our users. If you did not signup to be a member, please send an email to <a href=\"mailto:%Site.Email%\">%Site.Email%</a>.",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "Forums.NewForumPost",
										   Subject = "%Site.Name%. New Post Notification.",
										   Body = "<p><a href=\"%Site.URL%\">%Site.Name%</a> <br /><br />A new post has been created in the topic <a href=\"%Forums.TopicURL%\">\"%Forums.TopicName%\"</a> at <a href=\"%Forums.ForumURL%\">\"%Forums.ForumName%\"</a> forum.<br /><br />Click <a href=\"%Forums.TopicURL%\">here</a> for more info.<br /><br />Post author: %Forums.PostAuthor%<br />Post body: %Forums.PostBody%</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "Forums.NewForumTopic",
										   Subject = "%Site.Name%. New Topic Notification.",
										   Body = "<p><a href=\"%Site.URL%\">%Site.Name%</a> <br /><br />A new topic <a href=\"%Forums.TopicURL%\">\"%Forums.TopicName%\"</a> has been created at <a href=\"%Forums.ForumURL%\">\"%Forums.ForumName%\"</a> forum.<br /><br />Click <a href=\"%Forums.TopicURL%\">here</a> for more info.</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "NewUser.Notification",
										   Subject = "New user registration",
										   Body = "<p><a href=\"%Site.URL%\">%Site.Name%</a> <br /><br />A new user registered with your site. Below are the user's details:<br />Full name: %User.FullName%<br />Email: %User.Email%</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "News.NewsComment",
										   Subject = "%Site.Name%. New news comment.",
										   Body = "<p><a href=\"%Site.URL%\">%Site.Name%</a> <br /><br />A new news comment has been created for news \"%NewsComment.NewsTitle%\".</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "NewsLetterSubscription.ActivationMessage",
										   Subject = "%Site.Name%. Subscription activation message.",
										   Body = "<p><a href=\"%NewsLetterSubscription.ActivationUrl%\">Click here to confirm your subscription to our list.</a></p><p>If you received this email by mistake, simply delete it.</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "NewsLetterSubscription.DeactivationMessage",
										   Subject = "%Site.Name%. Subscription deactivation message.",
										   Body = "<p><a href=\"%NewsLetterSubscription.DeactivationUrl%\">Click here to unsubscribe from news letters.</a></p><p>If you received this email by mistake, simply delete it.</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
								   new MessageTemplate
									   {
										   Name = "Service.EmailAFriend",
										   Subject = "%Site.Name%. Referred",
										   Body = "<p><a href=\"%Site.URL%\"> %Site.Name%</a> <br /><br />%EmailAFriend.Email% %EmailAFriend.PersonalMessage%<br /><br />%Site.Name%</p>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
									   },
                                   new MessageTemplate
                                       {
                                           Name = "RFQ.SubmittedRFQForBuyer",
										   Subject = @"New RFQ #%RFQ.RFQNo% Created",
										   Body = @"<h3>Good day!</h3><span>RFQ # <span style='color: #303F9F; font-weight: bold'>%RFQ.RFQNo%</span> has been created.</span></br><span><b>Requester:</b>&nbsp;&nbsp;%RFQ.Requester%</span></br><span><b>Department:</b>&nbsp;&nbsp;%RFQ.Requester.Department%</span></br></br></br><table border='1' cellpadding='0' cellspacing='0' width='100%' style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'><thead><tr><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>RFQ Line No.</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='60%'>Item Description</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Quantity</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Maker</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Maker PN</th></tr></thead><tbody>%RFQ.Lines.Table%<tr><td colspan='5' style='font-weight: normal; border: 1px solid #dddddd; text-align: center; padding: 5px;'></td></tr></tbody></table></br></br><a href='%RFQ.Details.Url%'>Click to view quote details</a></br></br></br><span style='font-weight: bold; color: #D32F2F; text-decoration: underline;'>Vendor should indicate the ff in their quote:</span></br><span>Quotation number</span></br><span>Quote Validity</span></br><span>Lead Time</span></br><span>Warranty</span></br><span>Incoterm</span></br><span>BOM (Bill of Materials)*</span></br>	<span>Packaging dimension & weight*</span></br><span>Country of Origin/Pick-up Location*</span></br><span>*if needed</span></br></br></br><span style='font-weight: bold; color: #4CAF50'>This is an automated mail notification.</span>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "RFQ.SubmittedRFQForRequestor",
										   Subject = @"New RFQ #%RFQ.RFQNo% Created",
										   Body = @"<h3>Good day!</h3><span>RFQ # <span style='color: #303F9F; font-weight: bold'>%RFQ.RFQNo%</span> has been created.</span> </br><span><b>Requester:</b>&nbsp;&nbsp;%RFQ.Requester%</span></br><span><b>Department:</b>&nbsp;&nbsp;%RFQ.Requester.Department%</span></br></br></br>	<table border='1' cellpadding='0' cellspacing='0' width='100%' style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'><thead><tr><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>RFQ Line No.</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='60%'>Item Description</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Quantity</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Maker</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Maker PN</th></tr></thead><tbody>%RFQ.Lines.Table%<tr ><td colspan='5' style='font-weight: normal; border: 1px solid #dddddd; text-align: center; padding: 5px;'></td></tr></tbody></table></br></br><a href='%RFQ.Details.Url%'>Click to view quote details</a></br></br></br><span style='font-weight: bold; color: #4CAF50'>This is an automated mail notification.</span>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "RFQ.BuyerChanged",
										   Subject = @"RFQ #%RFQ.RFQNo% Buyer Reassigned",
										   Body = @"<h3>Good day!</h3><span>RFQ # <span style='color: #303F9F; font-weight: bold'>%RFQ.RFQNo%</span> has been reassigned to <span style='color: #303F9F; font-weight: bold'>%RFQ.Buyer%</span>.</span></br><span><b>Requester:</b>&nbsp;&nbsp;%RFQ.Requester%</span></br><span><b>Department:</b>&nbsp;&nbsp;%RFQ.Requester.Department%</span></br></br></br><table border='1' cellpadding='0' cellspacing='0' width='100%' style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'><thead><tr><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>RFQ Line No.</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='60%'>Item Description</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Quantity</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Maker</th><th style='background-color: #757575; color: #FFFFFF; border: 1px solid #dddddd; text-align: left; padding: 5px;' width='10%'>Maker PN</th></tr></thead><tbody>%RFQ.Lines.Table%<tr><td colspan='5' style='font-weight: normal; border: 1px solid #dddddd; text-align: center; padding: 5px;'></td></tr></tbody></table></br></br><span style='font-weight: bold; color: #D32F2F; text-decoration: underline;'>Vendor should indicate the ff in their quote:</span></br><span>Quotation number</span></br><span>Quote Validity</span></br><span>Lead Time</span></br><span>Warranty</span></br><span>Incoterm</span></br><span>BOM (Bill of Materials)*</span></br><span>Packaging dimension & weight*</span></br><span>Country of Origin/Pick-up Location*</span></br><span>*if needed</span></br></br></br><a href='%RFQ.Details.Url%'>Click to view quote details</a>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "RFQLine.NoteToBuyerChanged",
										   Subject = @"RFQ #%RFQ.RFQNo% Line #%RFQLine.RFQLineNo% Update",
										   Body = @"<h3>Good day!</h3><span>RFQ # <span style='color: #303F9F; font-weight: bold'>%RFQ.RFQNo%</span> has been updated.&nbsp;&nbsp;<a href='%RFQ.Details.Url%'>Click to see changes.</a></span></br></br><span style='font-weight: bold; color: #4CAF50'>This is an automated mail notification.</span>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "RFQLine.Deleted",
										   Subject = @"RFQ #%RFQ.RFQNo% Line #%RFQLine.RFQLineNo% Deleted",
										   Body = @"<h3>Good day!</h3><span><span style='font-weight: bold'>RFQ #</span> <span style='color: #303F9F; font-weight: bold'>%RFQ.RFQNo%</span> <span style='font-weight: bold'>Line #</span> <span style='color: #303F9F; font-weight: bold'>%RFQLine.RFQLineNo%</span> has been deleted.</span></br><span><b>Item Description:</b>&nbsp;&nbsp;%RFQLine.ItemDescription%</span></br></br><span style='font-weight: bold; color: #4CAF50'>This is an automated mail notification.</span>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "QUOTATION.Uploaded",
										   Subject = @"RFQ #%RFQ.RFQNo% Line #%RFQLine.RFQLineNo%  Quotation",
										   Body = @"<h3>Good day!</h3><span>Quotation has been uploaded.</span></br></br><span><b>RFQ Number:</b>&nbsp;&nbsp;<span style='font-weight: bold; color: #303F9F'>%RFQ.RFQNo%</span></span></br><span><b>RFQ Line No:</b>&nbsp;&nbsp;%RFQLine.RFQLineNo%</span></br><span><b>Item Description:</b>&nbsp;&nbsp;%RFQLine.ItemDescription%</span></br></br><a href='%RFQ.Details.Url%'>Click to view quote details</a></br></br></br><span style='font-weight: bold; color: #4CAF50'>This is an automated mail notification.</span>",
										   IsActive = true,
										   EmailAccountId = eaGeneral.Id,
                                       },
                                       new MessageTemplate()
                                       {
                                           Name = "RFQLine.SendEveryMonday",
                                           Subject = "Open RFQ Report",
                                           Body = "<span>Good Day,</span><br/><br/><span>This is the report of all Open RFQ. Please see attachment above.</span><br/><span>Report Creation Date : %Report.CreatedDate% </span><br/><br/><br/><span style='color:red;font-size:14px'>This email is auto generated.Please don't reply.</span>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       }
							   };
			messageTemplates.ForEach(mt => _messageTemplateRepository.Insert(mt));

		}

		protected virtual void InstallTopics()
		{
			var topics = new List<Topic>
							   {
								   new Topic
									   {
										   SystemName = "AboutUs",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "About Us",
										   Body = "<p>Put your &quot;About Us&quot; information here. You can edit this in the admin site.</p>"
									   },
								   new Topic
									   {
										   SystemName = "ConditionsOfUse",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "Conditions of use",
										   Body = "<p>Put your conditions of use information here. You can edit this in the admin site.</p>"
									   },
								   new Topic
									   {
										   SystemName = "ContactUs",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "",
										   Body = "<p>Put your contact information here. You can edit this in the admin site.</p>"
									   },
								   new Topic
									   {
										   SystemName = "ForumWelcomeMessage",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "Forums",
										   Body = "<p>Put your welcome message here. You can edit this in the admin site.</p>"
									   },
								   new Topic
									   {
										   SystemName = "HomePageText",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "Welcome to our site",
										   Body = "<p>Describe the application here.</p><p>If you have questions, see the <a href=\"http://jeepme.com/\">Documentation</a>, or post in the <a href=\"http://jeepme.com/\">Forums</a> at <a href=\"http://jeepme\">JEEPME</a></p>"
									   },
								   new Topic
									   {
										   SystemName = "LoginRegistrationInfo",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "About login / registration",
										   Body = "<p>Put your login / registration information here. You can edit this in the admin site.</p>"
									   },
								   new Topic
									   {
										   SystemName = "PrivacyInfo",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   Title = "Privacy policy",
										   Body = "<p>Put your privacy policy information here. You can edit this in the admin site.</p>"
									   },
							   };
			topics.ForEach(t => _topicRepository.Insert(t));

		}

		protected virtual void InstallSettings()
		{
			EngineContext.Current.Resolve<IConfigurationProvider<PdfSettings>>()
				.SaveSettings(new PdfSettings()
				{
					Enabled = true,
					LetterPageSizeEnabled = false,
					FontFileName = "FreeSerif.ttf",
				});

			EngineContext.Current.Resolve<IConfigurationProvider<CommonSettings>>()
				.SaveSettings(new CommonSettings()
				{
					UseSystemEmailForContactUsForm = true,
					UseStoredProceduresIfSupported = false,
					SitemapEnabled = true,
					SitemapIncludeTopics = true,
					DisplayJavaScriptDisabledWarning = false,
					UseFullTextSearch = false,
					FullTextMode = FulltextSearchMode.ExactMatch,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<SeoSettings>>()
				.SaveSettings(new SeoSettings()
				{
					PageTitleSeparator = ". ",
					PageTitleSeoAdjustment = PageTitleSeoAdjustment.PagenameAfterSitename,
					DefaultTitle = "RFQ",
					DefaultMetaKeywords = "",
					DefaultMetaDescription = "",
					ConvertNonWesternChars = false,
					AllowUnicodeCharsInUrls = true,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<AdminAreaSettings>>()
				.SaveSettings(new AdminAreaSettings()
				{
					GridPageSize = 15,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<LocalizationSettings>>()
				.SaveSettings(new LocalizationSettings()
				{
					DefaultAdminLanguageId = _languageRepository.Table.Where(l => l.Name == "English").Single().Id,
					UseImagesForLanguageSelection = false,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<UserSettings>>()
				.SaveSettings(new UserSettings()
				{
					UsernamesEnabled = true,
					CheckUsernameAvailabilityEnabled = true,
					AllowUsersToChangeUsernames = true,
					DefaultPasswordFormat = PasswordFormat.Hashed,
					HashedPasswordFormat = "SHA1",
					PasswordMinLength = 6,
					UserRegistrationType = UserRegistrationType.Standard,
					AllowUsersToUploadAvatars = true,
					AvatarMaximumSizeBytes = 20000,
					DefaultAvatarEnabled = true,
					ShowUsersLocation = false,
					ShowUsersJoinDate = false,
					AllowViewingProfiles = true,
					NotifyNewUserRegistration = false,
					UserNameFormat = UserNameFormat.ShowFullNames,
					GenderEnabled = false,
                    EmployeeIDRequired = true,
					DateOfBirthEnabled = false,
					CompanyEnabled = false,
					StreetAddressEnabled = false,
					StreetAddress2Enabled = false,
					ZipPostalCodeEnabled = false,
					CityEnabled = false,
					CountryEnabled = false,
					StateProvinceEnabled = false,
					PhoneEnabled = false,
					FaxEnabled = false,
					NewsletterEnabled = false,
					HideNewsletterBlock = true,
					OnlineUserMinutes = 20,
					SiteLastVisitedPage = true,
                    DepartmentEnabled = true
				});

			EngineContext.Current.Resolve<IConfigurationProvider<MediaSettings>>()
				.SaveSettings(new MediaSettings()
				{
					AvatarPictureSize = 85,
					MaximumImageSize = 1280,
					DefaultPictureZoomEnabled = false,
					DefaultImageQuality = 100,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<SiteInformationSettings>>()
				.SaveSettings(new SiteInformationSettings()
				{
					SiteName = "Request For Quote (RFQ)",
					SiteUrl = "http://jeepme.com/",
					SiteClosed = false,
					SiteClosedAllowForAdmins = false,
					DefaultSiteThemeForDesktops = "Template",
					AllowUserToSelectTheme = true,
					MobileDevicesSupported = false,
					DefaultSiteThemeForMobileDevices = "Mobile",
					EmulateMobileDevice = false,
					DisplayMiniProfilerInPublicSite = false,
					DisplayEuCookieLawWarning = false,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<CurrencySettings>>()
				.SaveSettings(new CurrencySettings()
				{
					PrimarySiteCurrencyId = _currencyRepository.Table.Where(c => c.CurrencyCode == "USD").Single().Id,
					PrimaryExchangeRateCurrencyId = _currencyRepository.Table.Where(c => c.CurrencyCode == "USD").Single().Id,
					ActiveExchangeRateProviderSystemName = "CurrencyExchange.MoneyConverter",
					AutoUpdateEnabled = false,
					LastUpdateTime = 0
				});

			EngineContext.Current.Resolve<IConfigurationProvider<MeasureSettings>>()
				.SaveSettings(new MeasureSettings()
				{
					BaseDimensionId = _measureDimensionRepository.Table.Where(m => m.SystemKeyword == "inches").Single().Id,
					BaseWeightId = _measureWeightRepository.Table.Where(m => m.SystemKeyword == "lb").Single().Id,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<MessageTemplatesSettings>>()
				.SaveSettings(new MessageTemplatesSettings()
				{
					CaseInvariantReplacement = false,
					Color1 = "#b9babe",
					Color2 = "#ebecee",
					Color3 = "#dde2e6",
				});

			EngineContext.Current.Resolve<IConfigurationProvider<SecuritySettings>>()
				.SaveSettings(new SecuritySettings()
				{
					ForceSslForAllPages = false,
					EncryptionKey = "273ece6f97dd844d",
					AdminAreaAllowedIpAddresses = null
				});

			EngineContext.Current.Resolve<IConfigurationProvider<FileSystemSettings>>()
				.SaveSettings(new FileSystemSettings()
				{
				});

			EngineContext.Current.Resolve<IConfigurationProvider<DateTimeSettings>>()
				.SaveSettings(new DateTimeSettings()
				{
					DefaultSiteTimeZoneId = "",
					AllowUsersToSetTimeZone = false
				});

			EngineContext.Current.Resolve<IConfigurationProvider<NewsSettings>>()
				.SaveSettings(new NewsSettings()
				{
					Enabled = true,
					AllowNotRegisteredUsersToLeaveComments = true,
					NotifyAboutNewNewsComments = false,
					ShowNewsOnMainPage = true,
					MainPageNewsCount = 3,
					NewsArchivePageSize = 10,
					ShowHeaderRssUrl = false,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<ForumSettings>>()
				.SaveSettings(new ForumSettings()
				{
					ForumsEnabled = true,
					RelativeDateTimeFormattingEnabled = true,
					AllowUsersToDeletePosts = false,
					AllowUsersToEditPosts = false,
					AllowUsersToManageSubscriptions = false,
					AllowGuestsToCreatePosts = false,
					AllowGuestsToCreateTopics = false,
					TopicSubjectMaxLength = 450,
					PostMaxLength = 4000,
					StrippedTopicMaxLength = 45,
					TopicsPageSize = 10,
					PostsPageSize = 10,
					SearchResultsPageSize = 10,
					LatestUserPostsPageSize = 10,
					ShowUsersPostCount = true,
					ForumEditor = EditorType.BBCodeEditor,
					SignaturesEnabled = true,
					AllowPrivateMessages = false,
					ShowAlertForPM = false,
					PrivateMessagesPageSize = 10,
					ForumSubscriptionsPageSize = 10,
					NotifyAboutPrivateMessages = false,
					PMSubjectMaxLength = 450,
					PMTextMaxLength = 4000,
					HomePageActiveDiscussionsTopicCount = 5,
					ActiveDiscussionsPageTopicCount = 50,
					ActiveDiscussionsFeedEnabled = false,
					ActiveDiscussionsFeedCount = 25,
					ForumFeedsEnabled = false,
					ForumFeedCount = 10,
					ForumSearchTermMinimumLength = 3,
				});

			EngineContext.Current.Resolve<IConfigurationProvider<EmailAccountSettings>>()
				.SaveSettings(new EmailAccountSettings()
				{
					DefaultEmailAccountId = _emailAccountRepository.Table.FirstOrDefault().Id
				});
		}

		protected virtual void InstallForums()
		{
			var forumGroup = new ForumGroup()
			{
				Name = "General",
				Description = "",
				DisplayOrder = 5,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};

			_forumGroupRepository.Insert(forumGroup);

			var usingXXXForum = new Forum()
			{
				ForumGroup = forumGroup,
				Name = "Using XXX",
				Description = "Discuss the procedures to use XXX",
				NumTopics = 0,
				NumPosts = 0,
				LastPostUserId = 0,
				LastPostTime = null,
				DisplayOrder = 10,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};
			_forumRepository.Insert(usingXXXForum);

			var generalForum = new Forum()
			{
				ForumGroup = forumGroup,
				Name = "General Topics",
				Description = "General Discussion",
				NumTopics = 0,
				NumPosts = 0,
				LastPostTime = null,
				DisplayOrder = 20,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};
			_forumRepository.Insert(generalForum);
		}

		protected virtual void InstallNews()
		{
			var defaultLanguage = _languageRepository.Table.FirstOrDefault();
			var news = new List<NewsItem>
								{
									new NewsItem
										{
											 AllowComments = true,
											 Language = defaultLanguage,
											 Title = "SSG XXX Release!",
											 Short = "SSG short release message",
											 Full = "SSG full release message",
											 Published  = true,
											 CreatedOnUtc = DateTime.UtcNow,
										},
								};
			news.ForEach(n => _newsItemRepository.Insert(n));

		}

		protected virtual void InstallPolls()
		{
			var defaultLanguage = _languageRepository.Table.FirstOrDefault();
			var poll1 = new Poll
			{
				Language = defaultLanguage,
				Name = "Do you like XXX?",
				SystemKeyword = "RightColumnPoll",
				Published = true,
				DisplayOrder = 1,
			};
			poll1.PollAnswers.Add(new PollAnswer()
			{
				Name = "Excellent",
				DisplayOrder = 1,
			});
			poll1.PollAnswers.Add(new PollAnswer()
			{
				Name = "Good",
				DisplayOrder = 2,
			});
			poll1.PollAnswers.Add(new PollAnswer()
			{
				Name = "Poor",
				DisplayOrder = 3,
			});
			poll1.PollAnswers.Add(new PollAnswer()
			{
				Name = "Very bad",
				DisplayOrder = 4,
			});
			_pollRepository.Insert(poll1);
		}

		protected virtual void InstallActivityLogTypes()
		{
			var activityLogTypes = new List<ActivityLogType>()
									  {
										  //admin area activities
										  new ActivityLogType
											  {
												  SystemKeyword = "AddNewUser",
												  Enabled = true,
												  Name = "Add a new user"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "AddNewUserRole",
												  Enabled = true,
												  Name = "Add a new user role"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "AddNewSetting",
												  Enabled = true,
												  Name = "Add a new setting"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "AddNewWidget",
												  Enabled = true,
												  Name = "Add a new widget"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "DeleteUser",
												  Enabled = true,
												  Name = "Delete a user"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "DeleteUserRole",
												  Enabled = true,
												  Name = "Delete a user role"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "DeleteSetting",
												  Enabled = true,
												  Name = "Delete a setting"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "DeleteWidget",
												  Enabled = true,
												  Name = "Delete a widget"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "EditUser",
												  Enabled = true,
												  Name = "Edit a user"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "EditUserRole",
												  Enabled = true,
												  Name = "Edit a user role"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "EditSettings",
												  Enabled = true,
												  Name = "Edit setting(s)"
											  },
										  new ActivityLogType
											  {
												  SystemKeyword = "EditWidget",
												  Enabled = true,
												  Name = "Edit a widget"
											  },
									  };
			activityLogTypes.ForEach(alt => _activityLogTypeRepository.Insert(alt));
		}

		protected virtual void InstallScheduleTasks()
		{
			var tasks = new List<ScheduleTask>()
			{
			    
				new ScheduleTask()
				{
					Name = "Send emails",
					Seconds = 60,
					Type = "SSG.Services.Messages.QueuedMessagesSendTask, SSG.Services",
					Enabled = true,
					StopOnError = false,
				},
				new ScheduleTask()
				{
					Name = "Keep alive",
					Seconds = 300,
					Type = "SSG.Services.Common.KeepAliveTask, SSG.Services",
					Enabled = true,
					StopOnError = false,
				},
				new ScheduleTask()
				{
					Name = "Delete guests",
					Seconds = 600,
					Type = "SSG.Services.Users.DeleteGuestsTask, SSG.Services",
					Enabled = true,
					StopOnError = false,
				},
				new ScheduleTask()
				{
					Name = "Clear cache",
					Seconds = 600,
					Type = "SSG.Services.Caching.ClearCacheTask, SSG.Services",
					Enabled = false,
					StopOnError = false,
				},
				new ScheduleTask()
				{
					Name = "Update currency exchange rates",
					Seconds = 900,
					Type = "SSG.Services.Directory.UpdateExchangeRateTask, SSG.Services",
					Enabled = true,
					StopOnError = false,
				},
			    new ScheduleTask()
			    {
			        Name = "Send Every Monday Report",
			        Seconds = 60,
			        Type = "SSG.Services.Messages.SendReportEveryMondayTask, SSG.Services",
			        Enabled = true,
			        StopOnError = false
			    },
                
			};

			tasks.ForEach(x => _scheduleTaskRepository.Insert(x));
		}

        

		#endregion

        #region RFQSeedData

        protected virtual void InstallRFQStatus()
        {
            var rfqStates = new List<RFQStatus>()
            {
                new RFQStatus()
                {
                    Name = "Draft",
                    Active = true,
                    DateCreatedOnUtc = DateTime.UtcNow,
                    DateUpdatedOnUtc = DateTime.UtcNow,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 1
                },
                new RFQStatus()
                {
                    Name = "Submitted",
                    Active = true,
                    DateCreatedOnUtc = DateTime.UtcNow,
                    DateUpdatedOnUtc = DateTime.UtcNow,
                    CreatedByUserId = 1,
                    UpdatedByUserId = 1
                }
            };

            rfqStates.ForEach(x => _rfqStatusRepository.Insert(x));
        }

        protected virtual void InstallRFQLineStatus()
        {
            var lineStatusNames = new List<string>()
            {
                "Open", "Closed", "Deleted"
            };

            lineStatusNames.ForEach(x => _rfqLineStatusRepository.Insert(new RFQLineStatus() { Name = x, Active = true, DateCreatedOnUtc = DateTime.UtcNow, DateUpdatedOnUtc = DateTime.UtcNow, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        protected virtual void InstallDefaultUserDepartments()
        {
            var adminUser = _userRepository.Table.Where(u => u.Username.Equals("antonio_amiel.yap@teradyne.com")).FirstOrDefault();

            if (adminUser != null)
            {
                adminUser.DepartmentId = _departmentRepository.Table.Where(d => d.Name.Equals("BPIT")).FirstOrDefault().Id;
                _userRepository.Update(adminUser);
            }

            var buyerDepartmentId = _departmentRepository.Table.Where(d => d.Name.Equals("Cebu Planning")).FirstOrDefault().Id;

            //Dianne
            var dianne = _userRepository.Table.Where(u => u.Username.Equals("delacru")).FirstOrDefault();

            if (dianne != null)
            {
                dianne.DepartmentId = buyerDepartmentId;
                _userRepository.Update(dianne);
            }

            //Rulther
            var rulther = _userRepository.Table.Where(u => u.Username.Equals("boncaler")).FirstOrDefault();

            if (rulther != null)
            {
                rulther.DepartmentId = buyerDepartmentId;
                _userRepository.Update(rulther);
            }

            //Shiela
            var shiela = _userRepository.Table.Where(u => u.Username.Equals("tutors")).FirstOrDefault();

            if (shiela != null)
            {
                shiela.DepartmentId = buyerDepartmentId;
                _userRepository.Update(shiela);
            }

            //Lloyd
            var lloyd = _userRepository.Table.Where(u => u.Username.Equals("perezllo")).FirstOrDefault();

            if (lloyd != null)
            {
                lloyd.DepartmentId = buyerDepartmentId;
                _userRepository.Update(lloyd);
            }
        }

        protected virtual void InstallDepartments()
        {
            var departmentNames = new List<string>()
            {
                "CE-MFG","CE-NPT","CE-STG&LEG","CE-EST","HR","ADC","MKTG","FLX","UFLX","J750",
                "FINANCE","PMO","ADI","HDD","PBT Rep","LEG","OEM","NXT","ETS","BPIT","PSG",
                "FACILITIES","CST","SMT","CMT","GTT","GWT","ATEOPS","PBT Mfg","Alabang Office",
                "RSG","Admin","Cebu Planning"
            };

            departmentNames.ForEach(x => _departmentRepository.Insert(new Department() { Name = x, Active = true, DateCreatedOnUtc = DateTime.UtcNow, DateUpdatedOnUtc = DateTime.UtcNow, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        protected virtual void InstallBuyerDepartments()
        {
            var departments = _departmentRepository.Table.Where(d => d.Active == true).AsEnumerable().ToList();

            //Shiela...
            var shiela = _userRepository.Table.Where(u => u.Username.Equals("tutors")).FirstOrDefault();

            if (shiela != null)
            {
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("CE-MFG")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("CE-NPT")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("CE-STG&LEG")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("CE-EST")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("FLX")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("UFLX")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("J750")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("PMO")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("Alabang Office")).FirstOrDefault());
                shiela.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("RSG")).FirstOrDefault());

                _userRepository.Update(shiela);
            }

            //Rulther...
            var rulther = _userRepository.Table.Where(u => u.Username.Equals("boncaler")).FirstOrDefault();

            if (rulther != null)
            {
                rulther.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("HR")).FirstOrDefault());
                rulther.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("FACILITIES")).FirstOrDefault());

                _userRepository.Update(rulther);
            }

            //Dianne...
            var dianne = _userRepository.Table.Where(u => u.Username.Equals("delacru")).FirstOrDefault();

            if (dianne != null)
            {
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("ADC")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("MKTG")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("FINANCE")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("ADI")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("HDD")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("PBT Rep")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("LEG")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("OEM")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("NXT")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("ETS")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("BPIT")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("PSG")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("CST")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("SMT")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("CMT")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("GTT")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("GWT")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("PBT Mfg")).FirstOrDefault());
                dianne.BuyerDepartments.Add(departments.Where(d => d.Name.Equals("Admin")).FirstOrDefault());

                _userRepository.Update(dianne);
            }

        }

        protected virtual void InstallRFQLineType()
        {
            var lineTypes = new List<string>()
            {
                "Goods",
                "AMT2"
            };

            lineTypes.ForEach(x => _rfqLineTypeRepository.Insert(new RFQLineType() { Name = x, Active = true, DateCreatedOnUtc = DateTime.UtcNow, DateUpdatedOnUtc = DateTime.UtcNow, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        protected virtual void InstallRFQLineFormType()
        {
            var formTypes = new List<string>()
            {
                "General Form",
                "Test Equipment Rental And Calibration Form"
            };

            formTypes.ForEach(x => _rfqLineFormTypeRepository.Insert(new RFQLineFormType() { Name = x, Active = true, DateCreatedOnUtc = DateTime.UtcNow, DateUpdatedOnUtc = DateTime.UtcNow, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        protected virtual void InstallEquipmentPurchaseType()
        {
            var equipmentPurchaseTypes = new List<string>()
            {
                "New",
                "Refurbished",
                "Used"
            };

            equipmentPurchaseTypes.ForEach(x => _equipmentPurchaseTypeRepository.Insert(new EquipmentPurchaseType() { Name = x, Active = true, DateCreatedOnUtc = DateTime.UtcNow, DateUpdatedOnUtc = DateTime.UtcNow, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        protected virtual void InstallEquipmentCalibrationType()
        {
            var equipmentCalibrationTypes = new List<string>()
            {
                "In-house",
                "On site"
            };

            equipmentCalibrationTypes.ForEach(x => _equipmentCalibrationTypeRepository.Insert(new EquipmentCalibrationType() { Name = x, Active = true, DateCreatedOnUtc = DateTime.Now, DateUpdatedOnUtc = DateTime.Now, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        protected virtual void InstallUnitOfMeasurements()
        {
            var units = new List<string>()
            {
                "Bottle","Box","Carton","Dozen","Drum","Each","Foot","Gallon","Gram","Kilogram",
                "Liter","Lot","Meter","Ounce","Pack","Pail","Pair","Ream","Roll","Set",
                "Sheet","Unit"
            };

            units.ForEach(x => _uomRepository.Insert(new UOM() { Name = x, Active = true, DateCreatedOnUtc = DateTime.UtcNow, DateUpdatedOnUtc = DateTime.UtcNow, CreatedByUserId = 1, UpdatedByUserId = 1 }));
        }

        #endregion

        #region Methods

        public virtual void InstallData(string defaultUserEmail,
			string defaultUserPassword, bool installSampleData = true)
		{
			InstallMeasures();
			InstallLanguages();
			InstallCurrencies();
			InstallCountriesAndStates();
            InstallUsersAndUsers(defaultUserEmail, defaultUserPassword);
			InstallEmailAccounts();
			InstallMessageTemplates();
			InstallTopics();
			InstallSettings();
			InstallLocaleResources();
			InstallActivityLogTypes();
			HashDefaultUserPassword(defaultUserEmail, defaultUserPassword);
			InstallScheduleTasks();

			if (installSampleData)
			{
				InstallForums();
				InstallNews();
				InstallPolls();
			}

            //RFQ Seed data installation...
            InstallRFQStatus();
            InstallRFQLineStatus();
            InstallRFQLineType();
            InstallRFQLineFormType();
            InstallEquipmentPurchaseType();
            InstallEquipmentCalibrationType();
            InstallUnitOfMeasurements();
            InstallDepartments();
            InstallDefaultUserDepartments();
            InstallBuyerDepartments();
		}

		#endregion
	}
}