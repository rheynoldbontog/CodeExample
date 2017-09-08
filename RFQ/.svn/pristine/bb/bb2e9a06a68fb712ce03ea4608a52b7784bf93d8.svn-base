using System.Collections.Generic;
using SSG.Plugin.Feed.Froogle.Domain;

namespace SSG.Plugin.Feed.Froogle.Services
{
    public partial interface IGoogleService
    {
        void DeleteGoogleProduct(GoogleProductRecord googleProductRecord);

        IList<GoogleProductRecord> GetAll();

        GoogleProductRecord GetById(int googleProductRecordId);

        GoogleProductRecord GetByProductVariantId(int productVariantId);

        void InsertGoogleProductRecord(GoogleProductRecord googleProductRecord);

        void UpdateGoogleProductRecord(GoogleProductRecord googleProductRecord);

        IList<string> GetTaxonomyList();
    }
}
