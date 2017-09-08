﻿using System;
using SSG.Core;
using SSG.Core.Data;
using SSG.Data;

namespace SSG.Data
{
    public partial class EfDataProviderManager : BaseDataProviderManager
    {
        public EfDataProviderManager(DataSettings settings)
            : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {

            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new SSGException("Data Settings doesn't contain a providerName");

            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
                case "sqlce":
                    return new SqlCeDataProvider();
                case "mysql":
                    return new MySqlDataProvider();
                default:
                    throw new SSGException(string.Format("Not supported dataprovider name: {0}", providerName));
            }
        }

    }
}