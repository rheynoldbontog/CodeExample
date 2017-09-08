using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Domain.Users;
using SSG.Core.Domain.Forums;
using SSG.Services.Users;
using SSG.Services.Forums;
using SSG.Services.Helpers;
using SSG.Web.Framework.Controllers;
using SSG.Web.Framework.Security;
using SSG.Web.Models.Common;
using SSG.Web.Models.PrivateMessages;

namespace SSG.Web.Controllers
{
    [SSGHttpsRequirement(SslRequirement.Yes)]
    public partial class PrivateMessagesController : BaseSSGController
    {
        #region Fields

        private readonly IForumService _forumService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ForumSettings _forumSettings;
        private readonly UserSettings _userSettings;

        #endregion

        #region Constructors

        public PrivateMessagesController(IForumService forumService,
            IUserService userService,
            IWorkContext workContext, IDateTimeHelper dateTimeHelper,
            ForumSettings forumSettings, UserSettings userSettings)
        {
            this._forumService = forumService;
            this._userService = userService;
            this._workContext = workContext;
            this._dateTimeHelper = dateTimeHelper;
            this._forumSettings = forumSettings;
            this._userSettings = userSettings;
        }

        #endregion

        #region Utilities

        [NonAction]
        private bool AllowPrivateMessages()
        {
            return _forumSettings.AllowPrivateMessages;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index(int? page, string tab)
        {
            if (!AllowPrivateMessages())
            {
                return RedirectToRoute("HomePage");
            }

            if (_workContext.CurrentUser.IsGuest())
            {
                return new HttpUnauthorizedResult();
            }

            int inboxPage = 0;
            int sentItemsPage = 0;
            bool sentItemsTabSelected = false;

            switch (tab)
            {
                case "inbox":
                    if (page.HasValue)
                    {
                        inboxPage = page.Value;
                    }
                    break;
                case "sent":
                    if (page.HasValue)
                    {
                        sentItemsPage = page.Value;
                    }
                    sentItemsTabSelected = true;
                    break;
                default:
                    break;
            }

            var model = new PrivateMessageIndexModel()
            {
                InboxPage = inboxPage,
                SentItemsPage = sentItemsPage,
                SentItemsTabSelected = sentItemsTabSelected
            };

            return View(model);
        }

        //inbox tab
        [ChildActionOnly]
        public virtual ActionResult Inbox(int page, string tab)
        {
            if (page > 0)
            {
                page -= 1;
            }

            var pageSize = _forumSettings.PrivateMessagesPageSize;

            var list = _forumService.GetAllPrivateMessages(0, _workContext.CurrentUser.Id, null, null, false, string.Empty, page, pageSize);

            var inbox = new List<PrivateMessageModel>();

            foreach (var pm in list)
            {
                inbox.Add(new PrivateMessageModel()
                {
                    Id = pm.Id,
                    FromUserId = pm.FromUser.Id,
                    UserFromName = pm.FromUser.FormatUserName(),
                    AllowViewingFromProfile = _userSettings.AllowViewingProfiles && pm.FromUser != null && !pm.FromUser.IsGuest(),
                    ToUserId = pm.ToUser.Id,
                    UserToName = pm.ToUser.FormatUserName(),
                    AllowViewingToProfile = _userSettings.AllowViewingProfiles && pm.ToUser != null && !pm.ToUser.IsGuest(),
                    Subject = pm.Subject,
                    Message = pm.Text,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime( pm.CreatedOnUtc, DateTimeKind.Utc),
                    IsRead = pm.IsRead,
                });
            }

            var pagerModel = new PagerModel()
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "PrivateMessagesPaged",
                UseRouteLinks = true,
                RouteValues = new PrivateMessageRouteValues { page = page, tab = tab }
            };

            var model = new PrivateMessageListModel()
            {
                Messages = inbox,
                PagerModel = pagerModel
            };

            return PartialView(model);
        }

        //sent items tab
        [ChildActionOnly]
        public virtual ActionResult SentItems(int page, string tab)
        {
            if (page > 0)
            {
                page -= 1;
            }

            var pageSize = _forumSettings.PrivateMessagesPageSize;

            var list = _forumService.GetAllPrivateMessages(_workContext.CurrentUser.Id, 0, null, false, null, string.Empty, page, pageSize);

            var sentItems = new List<PrivateMessageModel>();

            foreach (var pm in list)
            {
                sentItems.Add(new PrivateMessageModel()
                {
                    Id = pm.Id,
                    FromUserId = pm.FromUser.Id,
                    UserFromName = pm.FromUser.FormatUserName(),
                    AllowViewingFromProfile = _userSettings.AllowViewingProfiles && pm.FromUser != null && !pm.FromUser.IsGuest(),
                    ToUserId = pm.ToUser.Id,
                    UserToName = pm.ToUser.FormatUserName(),
                    AllowViewingToProfile = _userSettings.AllowViewingProfiles && pm.ToUser != null && !pm.ToUser.IsGuest(),
                    Subject = pm.Subject,
                    Message = pm.Text,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(pm.CreatedOnUtc, DateTimeKind.Utc),
                    IsRead = pm.IsRead,
                });
            }

            var pagerModel = new PagerModel()
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "PrivateMessagesPaged",
                UseRouteLinks = true,
                RouteValues = new PrivateMessageRouteValues { page = page, tab = tab }
            };

            var model = new PrivateMessageListModel()
            {
                Messages = sentItems,
                PagerModel = pagerModel
            };

            return PartialView(model);
        }

        [HttpPost, FormValueRequired("delete-inbox"), ActionName("InboxUpdate")]
        public virtual ActionResult DeleteInboxPM(FormCollection formCollection)
        {
            foreach (var key in formCollection.AllKeys)
            {
                var value = formCollection[key];

                if (value.Equals("on") && key.StartsWith("pm", StringComparison.InvariantCultureIgnoreCase))
                {
                    var id = key.Replace("pm", "").Trim();
                    int privateMessageId = 0;

                    if (Int32.TryParse(id, out privateMessageId))
                    {
                        var pm = _forumService.GetPrivateMessageById(privateMessageId);
                        if (pm != null)
                        {
                            if (pm.ToUserId == _workContext.CurrentUser.Id)
                            {
                                pm.IsDeletedByRecipient = true;
                                _forumService.UpdatePrivateMessage(pm);
                            }
                        }
                    }
                }
            }
            return RedirectToRoute("PrivateMessages");
        }

        [HttpPost, FormValueRequired("mark-unread"), ActionName("InboxUpdate")]
        public virtual ActionResult MarkUnread(FormCollection formCollection)
        {
            foreach (var key in formCollection.AllKeys)
            {
                var value = formCollection[key];

                if (value.Equals("on") && key.StartsWith("pm", StringComparison.InvariantCultureIgnoreCase))
                {
                    var id = key.Replace("pm", "").Trim();
                    int privateMessageId = 0;

                    if (Int32.TryParse(id, out privateMessageId))
                    {
                        var pm = _forumService.GetPrivateMessageById(privateMessageId);
                        if (pm != null)
                        {
                            if (pm.ToUserId == _workContext.CurrentUser.Id)
                            {
                                pm.IsRead = false;
                                _forumService.UpdatePrivateMessage(pm);
                            }
                        }
                    }
                }
            }
            return RedirectToRoute("PrivateMessages");
        }

        //updates sent items (deletes PrivateMessages)
        [HttpPost, FormValueRequired("delete-sent"), ActionName("SentUpdate")]
        public virtual ActionResult DeleteSentPM(FormCollection formCollection)
        {
            foreach (var key in formCollection.AllKeys)
            {
                var value = formCollection[key];

                if (value.Equals("on") && key.StartsWith("si", StringComparison.InvariantCultureIgnoreCase))
                {
                    var id = key.Replace("si", "").Trim();
                    int privateMessageId = 0;

                    if (Int32.TryParse(id, out privateMessageId))
                    {
                        PrivateMessage pm = _forumService.GetPrivateMessageById(privateMessageId);
                        if (pm != null)
                        {
                            if (pm.FromUserId == _workContext.CurrentUser.Id)
                            {
                                pm.IsDeletedByAuthor = true;
                                _forumService.UpdatePrivateMessage(pm);
                            }
                        }
                    }
                }

            }
            return RedirectToRoute("PrivateMessages", new {tab = "sent"});
        }

        public virtual ActionResult SendPM(int toUserId, int? replyToMessageId)
        {
            if (!AllowPrivateMessages())
            {
                return RedirectToRoute("HomePage");
            }

            if (_workContext.CurrentUser.IsGuest())
            {
                return new HttpUnauthorizedResult();
            }

            var userTo = _userService.GetUserById(toUserId);

            if (userTo == null || userTo.IsGuest())
            {
                return RedirectToRoute("PrivateMessages");
            }

            var model = new SendPrivateMessageModel();
            model.ToUserId = userTo.Id;
            model.UserToName = userTo.FormatUserName();
            model.AllowViewingToProfile = _userSettings.AllowViewingProfiles && !userTo.IsGuest();

            if (replyToMessageId.HasValue)
            {
                var replyToPM = _forumService.GetPrivateMessageById(replyToMessageId.Value);
                if (replyToPM == null)
                {
                    return RedirectToRoute("PrivateMessages");
                }

                if (replyToPM.ToUserId == _workContext.CurrentUser.Id || replyToPM.FromUserId == _workContext.CurrentUser.Id)
                {
                    model.ReplyToMessageId = replyToPM.Id;
                    model.Subject = string.Format("Re: {0}", replyToPM.Subject);
                }
                else
                {
                    return RedirectToRoute("PrivateMessages");
                }
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SendPM(SendPrivateMessageModel model)
        {
            if (!AllowPrivateMessages())
            {
                return RedirectToRoute("HomePage");
            }

            if (_workContext.CurrentUser.IsGuest())
            {
                return new HttpUnauthorizedResult();
            }

            User toUser = null;
            var replyToPM = _forumService.GetPrivateMessageById(model.ReplyToMessageId);
            if (replyToPM != null)
            {
                if (replyToPM.ToUserId == _workContext.CurrentUser.Id || replyToPM.FromUserId == _workContext.CurrentUser.Id)
                {
                    toUser = replyToPM.FromUser;
                }
                else
                {
                    return RedirectToRoute("PrivateMessages");
                }
            }
            else
            {
                toUser = _userService.GetUserById(model.ToUserId);
            }

            if (toUser == null || toUser.IsGuest())
            {
                return RedirectToRoute("PrivateMessages");
            }
            model.ToUserId = toUser.Id;
            model.UserToName = toUser.FormatUserName();
            model.AllowViewingToProfile = _userSettings.AllowViewingProfiles && !toUser.IsGuest();

            if (ModelState.IsValid)
            {
                try
                {
                    string subject = model.Subject;
                    if (_forumSettings.PMSubjectMaxLength > 0 && subject.Length > _forumSettings.PMSubjectMaxLength)
                    {
                        subject = subject.Substring(0, _forumSettings.PMSubjectMaxLength);
                    }

                    var text = model.Message;
                    if (_forumSettings.PMTextMaxLength > 0 && text.Length > _forumSettings.PMTextMaxLength)
                    {
                        text = text.Substring(0, _forumSettings.PMTextMaxLength);
                    }

                    var nowUtc = DateTime.UtcNow;

                    var privateMessage = new PrivateMessage
                    {
                        ToUserId = toUser.Id,
                        FromUserId = _workContext.CurrentUser.Id,
                        Subject = subject,
                        Text = text,
                        IsDeletedByAuthor = false,
                        IsDeletedByRecipient = false,
                        IsRead = false,
                        CreatedOnUtc = nowUtc
                    };

                    _forumService.InsertPrivateMessage(privateMessage);

                    return RedirectToRoute("PrivateMessages", new { tab = "sent" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        public virtual ActionResult ViewPM(int privateMessageId)
        {
            if (!AllowPrivateMessages())
            {
                return RedirectToRoute("HomePage");
            }

            if (_workContext.CurrentUser.IsGuest())
            {
                return new HttpUnauthorizedResult();
            }

            var pm = _forumService.GetPrivateMessageById(privateMessageId);
            if (pm != null)
            {
                if (pm.ToUserId != _workContext.CurrentUser.Id && pm.FromUserId != _workContext.CurrentUser.Id)
                {
                    return RedirectToRoute("PrivateMessages");
                }

                if (!pm.IsRead && pm.ToUserId == _workContext.CurrentUser.Id)
                {
                    pm.IsRead = true;
                    _forumService.UpdatePrivateMessage(pm);
                }
            }
            else
            {
                return RedirectToRoute("PrivateMessages");
            }

            var model = new PrivateMessageModel()
            {
                Id = pm.Id,
                FromUserId = pm.FromUser.Id,
                UserFromName = pm.FromUser.FormatUserName(),
                AllowViewingFromProfile = _userSettings.AllowViewingProfiles && pm.FromUser != null && !pm.FromUser.IsGuest(),
                ToUserId = pm.ToUser.Id,
                UserToName = pm.ToUser.FormatUserName(),
                AllowViewingToProfile = _userSettings.AllowViewingProfiles && pm.ToUser != null && !pm.ToUser.IsGuest(),
                Subject = pm.Subject,
                Message = pm.FormatPrivateMessageText(),
                CreatedOn = _dateTimeHelper.ConvertToUserTime(pm.CreatedOnUtc, DateTimeKind.Utc),
                IsRead = pm.IsRead,
            };

            return View(model);
        }

        public virtual ActionResult DeletePM(int privateMessageId)
        {
            if (!AllowPrivateMessages())
            {
                return RedirectToRoute("HomePage");
            }

            if (_workContext.CurrentUser.IsGuest())
            {
                return new HttpUnauthorizedResult();
            }

            var pm = _forumService.GetPrivateMessageById(privateMessageId);
            if (pm != null)
            {
                if (pm.FromUserId == _workContext.CurrentUser.Id)
                {
                    pm.IsDeletedByAuthor = true;
                    _forumService.UpdatePrivateMessage(pm);
                }

                if (pm.ToUserId == _workContext.CurrentUser.Id)
                {
                    pm.IsDeletedByRecipient = true;
                    _forumService.UpdatePrivateMessage(pm);
                }
            }
            return RedirectToRoute("PrivateMessages");
        }

        #endregion
    }
}
