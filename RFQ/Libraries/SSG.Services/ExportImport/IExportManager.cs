using System.Collections.Generic;
using SSG.Core.Domain.Users;

namespace SSG.Services.ExportImport
{
    /// <summary>
    /// Export manager interface
    /// </summary>
    public interface IExportManager
    {
        /// <summary>
        /// Export user list to XLSX
        /// </summary>
        /// <param name="filePath">File path to use</param>
        /// <param name="users">Users</param>
        void ExportUsersToXlsx(string filePath, IList<User> users);
        
        /// <summary>
        /// Export user list to xml
        /// </summary>
        /// <param name="users">Users</param>
        /// <returns>Result in XML format</returns>
        string ExportUsersToXml(IList<User> users);
    }
}
