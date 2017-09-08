using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Users;
using SSG.Services.Common;
using SSG.Services.Media;
using SSG.Services.Messages;
using SSG.Services.Users;

namespace SSG.Services.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
public partial class ExportManager : IExportManager
    {
        #region Fields

        private readonly IPictureService _pictureService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly SiteInformationSettings _siteInformationSettings;

        #endregion

        #region Ctor

        public ExportManager(
            IPictureService pictureService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            SiteInformationSettings siteInformationSettings)
        {
            this._pictureService = pictureService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._siteInformationSettings = siteInformationSettings;
        }

        #endregion

        #region Utilities

        #endregion

        #region Methods

        /// <summary>
        /// Export user list to XLSX
        /// </summary>
        /// <param name="filePath">File path to use</param>
        /// <param name="users">Users</param>
        public virtual void ExportUsersToXlsx(string filePath, IList<User> users)
        {
            var newFile = new FileInfo(filePath);
            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(newFile))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Users");
                //Create Headers and format them
                var properties = new string[]
                    {
                        "UserId",
                        "UserGuid",
                        "Email",
                        "Username",
                        "PasswordStr",//why can't we use 'Password' name?
                        "PasswordFormatId",
                        "PasswordSalt",
                        "LanguageId",
                        "CurrencyId",
                        "TimeZoneId",
                        "Active",
                        "IsGuest",
                        "IsRegistered",
                        "IsAdministrator",
                        "IsForumModerator",
                        "FirstName",
                        "LastName",
                        "Gender",
                        "Company",
                        "StreetAddress",
                        "StreetAddress2",
                        "ZipPostalCode",
                        "City",
                        "CountryId",
                        "StateProvinceId",
                        "Phone",
                        "Fax",
                        "AvatarPictureId",
                        "ForumPostCount",
                        "Signature",
                    };
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var user in users)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = user.Id;
                    col++;

                    worksheet.Cells[row, col].Value = user.UserGuid;
                    col++;

                    worksheet.Cells[row, col].Value = user.Email;
                    col++;

                    worksheet.Cells[row, col].Value = user.Username;
                    col++;

                    worksheet.Cells[row, col].Value = user.Password;
                    col++;

                    worksheet.Cells[row, col].Value = user.PasswordFormatId;
                    col++;

                    worksheet.Cells[row, col].Value = user.PasswordSalt;
                    col++;

                    worksheet.Cells[row, col].Value = user.LanguageId.HasValue ? user.LanguageId.Value : 0;
                    col++;

                    worksheet.Cells[row, col].Value = user.CurrencyId.HasValue ? user.CurrencyId.Value : 0;
                    col++;

                    worksheet.Cells[row, col].Value = user.TimeZoneId;
                    col++;

                    worksheet.Cells[row, col].Value = user.Active;
                    col++;

                    //roles
                    worksheet.Cells[row, col].Value = user.IsGuest();
                    col++;

                    worksheet.Cells[row, col].Value = user.IsRegistered();
                    col++;

                    worksheet.Cells[row, col].Value = user.IsAdmin();
                    col++;

                    worksheet.Cells[row, col].Value = user.IsForumModerator();
                    col++;
                    
                    //attributes
                    var firstName = user.GetAttribute<string>(SystemUserAttributeNames.FirstName);
                    var lastName = user.GetAttribute<string>(SystemUserAttributeNames.LastName);
                    var gender = user.GetAttribute<string>(SystemUserAttributeNames.Gender);
                    var company = user.GetAttribute<string>(SystemUserAttributeNames.Company);
                    var streetAddress = user.GetAttribute<string>(SystemUserAttributeNames.StreetAddress);
                    var streetAddress2 = user.GetAttribute<string>(SystemUserAttributeNames.StreetAddress2);
                    var zipPostalCode = user.GetAttribute<string>(SystemUserAttributeNames.ZipPostalCode);
                    var city = user.GetAttribute<string>(SystemUserAttributeNames.City);
                    var countryId = user.GetAttribute<int>(SystemUserAttributeNames.CountryId);
                    var stateProvinceId = user.GetAttribute<int>(SystemUserAttributeNames.StateProvinceId);
                    var phone = user.GetAttribute<string>(SystemUserAttributeNames.Phone);
                    var fax = user.GetAttribute<string>(SystemUserAttributeNames.Fax);

                    var avatarPictureId = user.GetAttribute<int>(SystemUserAttributeNames.AvatarPictureId);
                    var forumPostCount = user.GetAttribute<int>(SystemUserAttributeNames.ForumPostCount);
                    var signature = user.GetAttribute<string>(SystemUserAttributeNames.Signature);

                    worksheet.Cells[row, col].Value = firstName;
                    col++;

                    worksheet.Cells[row, col].Value = lastName;
                    col++;

                    worksheet.Cells[row, col].Value = gender;
                    col++;

                    worksheet.Cells[row, col].Value = company;
                    col++;

                    worksheet.Cells[row, col].Value = streetAddress;
                    col++;

                    worksheet.Cells[row, col].Value = streetAddress2;
                    col++;

                    worksheet.Cells[row, col].Value = zipPostalCode;
                    col++;

                    worksheet.Cells[row, col].Value = city;
                    col++;

                    worksheet.Cells[row, col].Value = countryId;
                    col++;

                    worksheet.Cells[row, col].Value = stateProvinceId;
                    col++;

                    worksheet.Cells[row, col].Value = phone;
                    col++;

                    worksheet.Cells[row, col].Value = fax;
                    col++;

                    worksheet.Cells[row, col].Value = avatarPictureId;
                    col++;

                    worksheet.Cells[row, col].Value = forumPostCount;
                    col++;

                    worksheet.Cells[row, col].Value = signature;
                    col++;

                    row++;
                }








                // we had better add some document properties to the spreadsheet 

                // set some core property values
                xlPackage.Workbook.Properties.Title = string.Format("{0} users", _siteInformationSettings.SiteName);
                xlPackage.Workbook.Properties.Author = _siteInformationSettings.SiteName;
                xlPackage.Workbook.Properties.Subject = string.Format("{0} users", _siteInformationSettings.SiteName);
                xlPackage.Workbook.Properties.Keywords = string.Format("{0} users", _siteInformationSettings.SiteName);
                xlPackage.Workbook.Properties.Category = "Users";
                xlPackage.Workbook.Properties.Comments = string.Format("{0} users", _siteInformationSettings.SiteName);

                // set some extended property values
                xlPackage.Workbook.Properties.Company = _siteInformationSettings.SiteName;
                xlPackage.Workbook.Properties.HyperlinkBase = new Uri(_siteInformationSettings.SiteUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Export user list to xml
        /// </summary>
        /// <param name="users">Users</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportUsersToXml(IList<User> users)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Users");
            xmlWriter.WriteAttributeString("Version", SSGVersion.CurrentVersion);

            foreach (var user in users)
            {
                xmlWriter.WriteStartElement("User");
                xmlWriter.WriteElementString("UserId", null, user.Id.ToString());
                xmlWriter.WriteElementString("UserGuid", null, user.UserGuid.ToString());
                xmlWriter.WriteElementString("Email", null, user.Email);
                xmlWriter.WriteElementString("Username", null, user.Username);
                xmlWriter.WriteElementString("Password", null, user.Password);
                xmlWriter.WriteElementString("PasswordFormatId", null, user.PasswordFormatId.ToString());
                xmlWriter.WriteElementString("PasswordSalt", null, user.PasswordSalt);
                xmlWriter.WriteElementString("LanguageId", null, user.LanguageId.HasValue ? user.LanguageId.ToString() : "0");
                xmlWriter.WriteElementString("CurrencyId", null, user.CurrencyId.HasValue ? user.CurrencyId.ToString() : "0");
                xmlWriter.WriteElementString("TimeZoneId", null, user.TimeZoneId);
                xmlWriter.WriteElementString("Active", null, user.Active.ToString());


                xmlWriter.WriteElementString("IsGuest", null, user.IsGuest().ToString());
                xmlWriter.WriteElementString("IsRegistered", null, user.IsRegistered().ToString());
                xmlWriter.WriteElementString("IsAdministrator", null, user.IsAdmin().ToString());
                xmlWriter.WriteElementString("IsForumModerator", null, user.IsForumModerator().ToString());

                xmlWriter.WriteElementString("FirstName", null, user.GetAttribute<string>(SystemUserAttributeNames.FirstName));
                xmlWriter.WriteElementString("LastName", null, user.GetAttribute<string>(SystemUserAttributeNames.LastName));
                xmlWriter.WriteElementString("Gender", null, user.GetAttribute<string>(SystemUserAttributeNames.Gender));
                xmlWriter.WriteElementString("Company", null, user.GetAttribute<string>(SystemUserAttributeNames.Company));

                xmlWriter.WriteElementString("CountryId", null, user.GetAttribute<int>(SystemUserAttributeNames.CountryId).ToString());
                xmlWriter.WriteElementString("StreetAddress", null, user.GetAttribute<string>(SystemUserAttributeNames.StreetAddress));
                xmlWriter.WriteElementString("StreetAddress2", null, user.GetAttribute<string>(SystemUserAttributeNames.StreetAddress2));
                xmlWriter.WriteElementString("ZipPostalCode", null, user.GetAttribute<string>(SystemUserAttributeNames.ZipPostalCode));
                xmlWriter.WriteElementString("City", null, user.GetAttribute<string>(SystemUserAttributeNames.City));
                xmlWriter.WriteElementString("CountryId", null, user.GetAttribute<int>(SystemUserAttributeNames.CountryId).ToString());
                xmlWriter.WriteElementString("StateProvinceId", null, user.GetAttribute<int>(SystemUserAttributeNames.StateProvinceId).ToString());
                xmlWriter.WriteElementString("Phone", null, user.GetAttribute<string>(SystemUserAttributeNames.Phone));
                xmlWriter.WriteElementString("Fax", null, user.GetAttribute<string>(SystemUserAttributeNames.Fax));

                var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmail(user.Email);
                bool subscribedToNewsletters = newsletter != null && newsletter.Active;
                xmlWriter.WriteElementString("Newsletter", null, subscribedToNewsletters.ToString());

                xmlWriter.WriteElementString("AvatarPictureId", null, user.GetAttribute<int>(SystemUserAttributeNames.AvatarPictureId).ToString());
                xmlWriter.WriteElementString("ForumPostCount", null, user.GetAttribute<int>(SystemUserAttributeNames.ForumPostCount).ToString());
                xmlWriter.WriteElementString("Signature", null, user.GetAttribute<string>(SystemUserAttributeNames.Signature));
                
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }


        #endregion
    }
}
