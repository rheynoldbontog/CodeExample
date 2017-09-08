using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Web.Framework.Mvc;
using SSG.Web.Validators.PrivateMessages;

namespace SSG.Web.Models.PrivateMessages
{
    [Validator(typeof(SendPrivateMessageValidator))]
    public partial class SendPrivateMessageModel : BaseSSGEntityModel
    {
        public int ToUserId { get; set; }
        public string UserToName { get; set; }
        public bool AllowViewingToProfile { get; set; }

        public int ReplyToMessageId { get; set; }

        [AllowHtml]
        public string Subject { get; set; }

        [AllowHtml]
        public string Message { get; set; }
    }
}