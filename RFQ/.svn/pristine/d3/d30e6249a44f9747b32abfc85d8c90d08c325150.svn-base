using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSG.Services.RFQ;
using SSG.Core.Domain.Common;
using SSG.Core;
using SSG.Services.Security;
using SSG.Services.Users;
using System.Web.Mvc;
using SSG.Web.Models.RFQ;
using SSG.Core.Domain.RFQ;
using SSG.Web.Extensions;
using AutoMapper;
using SSG.Web.Models.FineUploader;
using System.IO;

namespace SSG.Web.Controllers
{
    public partial class FileAttachmentController : BaseSSGController
    {
        #region Fields

        private readonly IFileAttachmentService _fileAttachmentService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        #endregion

        #region ctor

        public FileAttachmentController(IFileAttachmentService fileAttachmentService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService,
            IUserService userService
            )
        {
            this._fileAttachmentService = fileAttachmentService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._userService = userService;
        }

        #endregion

        #region Methods

        public virtual ViewResult CreateLineAttachment(int rfqLineId, int parentIndex, int lastChildIndex)
        {
            var attachment = new FileAttachmentModel()
            {
                RFQLineId = rfqLineId,
                Index = lastChildIndex.ToString(),
                IsDeleted = false
            };

            return View("_CreateLineAttachment", attachment);
        }

        public virtual ViewResult CreateROHSDocumentAttachment(int rfqLineId, int parentIndex, int lastChildIndex)
        {
            var attachment = new FileAttachmentModel()
            {
                RFQLineId = rfqLineId,
                QuotationIndex = parentIndex.ToString(),
                Index = lastChildIndex.ToString(),
                IsDeleted = false
            };

            return View("_CreateROHSDocumentAttachment", attachment);
        }

        public virtual ViewResult CreateQuoteAttachment(int rfqLineId, int parentIndex, int lastChildIndex)
        {
            var attachment = new FileAttachmentModel()
            {
                RFQLineId = rfqLineId,
                QuotationIndex = parentIndex.ToString(),
                Index = lastChildIndex.ToString(),
                IsDeleted = false
            };

            return View("_CreateEditQuoteAttachment", attachment);
        }

        public virtual ViewResult CreateMSDSDocumentAttachment(int rfqLineId, int parentIndex, int lastChildIndex)
        {
            var attachment = new FileAttachmentModel()
            {
                RFQLineId = rfqLineId,
                QuotationIndex = parentIndex.ToString(),
                Index = lastChildIndex.ToString(),
                IsDeleted = false
            };

            return View("_CreateMSDSDocumentAttachment", attachment);
        }

        public virtual ViewResult CreateOtherDocumentAttachment(int rfqLineId, int parentIndex, int lastChildIndex)
        {
            var attachment = new FileAttachmentModel()
            {
                RFQLineId = rfqLineId,
                QuotationIndex = parentIndex.ToString(),
                Index = lastChildIndex.ToString(),
                IsDeleted = false
            };

            return View("_CreateOtherDocumentAttachment", attachment);
        }

        [HttpPost]
        public virtual FineUploaderResult UploadAttachment(FineUpload upload, string path)
        {
            var dir = Server.MapPath(path);
            var uuid = HttpContext.Request["qquuid"];

            return SaveUploadedFile(upload, dir, uuid, path);
        }

        private FineUploaderResult SaveUploadedFile(FineUpload upload, string dir, string uuid, string path)
        {
            var filePath = Path.Combine(dir, upload.Filename);
            try
            {
                upload.SaveAs(filePath, true, true);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }

            var fileName = upload.Filename;
            var fileUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content(path) + upload.Filename;

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FineUploaderResult(true, new { filename = fileName, fileurl = fileUrl });
        }

        #endregion
    }
}