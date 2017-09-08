using System;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Domain.Users;
using SSG.Services.Media;

namespace SSG.Web.Controllers
{
    public partial class DownloadController : BaseSSGController
    {
        private readonly IDownloadService _downloadService;
        private readonly IWorkContext _workContext;

        private readonly UserSettings _userSettings;

        public DownloadController(IDownloadService downloadService, IWorkContext workContext, UserSettings userSettings)
        {
            this._downloadService = downloadService;
            this._workContext = workContext;
            this._userSettings = userSettings;
        }

        public virtual ActionResult GetFileUpload(Guid downloadId)
        {
            var download = _downloadService.GetDownloadByGuid(downloadId);
            if (download == null)
                return Content("Download is not available any more.");

            if (download.UseDownloadUrl)
            {
                //return result
                return new RedirectResult(download.DownloadUrl);
            }
            else
            {
                if (download.DownloadBinary == null)
                    return Content("Download data is not available any more.");

                //return result
                string fileName = !String.IsNullOrWhiteSpace(download.Filename) ? download.Filename : downloadId.ToString();
                string contentType = !String.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : "application/octet-stream";
                return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension };
            }
        }

    }
}
