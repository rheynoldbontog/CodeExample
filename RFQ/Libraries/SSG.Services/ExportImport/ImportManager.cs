using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using SSG.Core;
using SSG.Services.Media;

namespace SSG.Services.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager : IImportManager
    {
        private readonly IPictureService _pictureService;

        public ImportManager(IPictureService pictureService)
        {
            this._pictureService = pictureService;
        }

        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }
    }
}
