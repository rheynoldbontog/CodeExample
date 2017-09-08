using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Core.Domain.Users;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Media;
using SSG.Services.Common;
using SSG.Services.Users;
using SSG.Services.Directory;
using SSG.Services.Forums;
using SSG.Services.Helpers;
using SSG.Services.Localization;
using SSG.Services.Media;
using SSG.Services.Seo;
using SSG.Web.Framework;
using SSG.Web.Framework.Security;
using SSG.Web.Models.Common;
using SSG.Web.Models.Profile;

namespace SSG.Web.Controllers
{
    [SSGHttpsRequirement(SslRequirement.No)]
    public partial class ProfileController : BaseSSGController
    {
        private readonly IForumService _forumService;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ICountryService _countryService;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ForumSettings _forumSettings;
        private readonly UserSettings _userSettings;
        private readonly MediaSettings _mediaSettings;

        public ProfileController(IForumService forumService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            ICountryService countryService,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            ForumSettings forumSettings,
            UserSettings userSettings,
            MediaSettings mediaSettings)
        {
            this._forumService = forumService;
            this._localizationService = localizationService;
            this._pictureService = pictureService;
            this._countryService = countryService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._forumSettings = forumSettings;
            this._userSettings = userSettings;
            this._mediaSettings = mediaSettings;
        }

        public virtual ActionResult Index(int? id, int? page)
        {
            var userId = 0;
            if (id.HasValue)
            {
                userId = id.Value;
            }

            var user = _userService.GetUserById(userId);
            if (!_userSettings.AllowViewingProfiles || (user == null || user.IsGuest()))
            {
                return RedirectToRoute("HomePage");
            }

            bool pagingPosts = false;
            int postsPage = 0;

            if (page.HasValue)
            {
                postsPage = page.Value;
                pagingPosts = true;
            }

            var name = user.FormatUserName();
            var title = string.Format(_localizationService.GetResource("Profile.ProfileOf"), name);

            var model = new ProfileIndexModel()
            {
                ProfileTitle = title,
                PostsPage = postsPage,
                PagingPosts = pagingPosts,
                UserProfileId = user.Id,
                ForumsEnabled = _forumSettings.ForumsEnabled
            };

            return View(model);
        }

        //profile info tab
        [ChildActionOnly]
        public virtual ActionResult Info(int userProfileId)
        {
            var user = _userService.GetUserById(userProfileId);
            if (user == null)
            {
                return RedirectToRoute("HomePage");
            }

            //avatar
            bool avatarEnabled = false;
            string avatarUrl = _pictureService.GetDefaultPictureUrl(_mediaSettings.AvatarPictureSize, PictureType.Avatar);
            if (_userSettings.AllowUsersToUploadAvatars)
            {
                avatarEnabled = true;

                var userAvatarId = user.GetAttribute<int>(SystemUserAttributeNames.AvatarPictureId);

                if (userAvatarId != 0)
                {
                    avatarUrl = _pictureService.GetPictureUrl(userAvatarId, _mediaSettings.AvatarPictureSize, false);
                }
                else
                {
                    if (!_userSettings.DefaultAvatarEnabled)
                    {
                        avatarEnabled = false;
                    }
                }
            }

            //location
            bool locationEnabled = false;
            string location = string.Empty;
            if (_userSettings.ShowUsersLocation)
            {
                locationEnabled = true;

                var countryId = user.GetAttribute<int>(SystemUserAttributeNames.CountryId);
                var country = _countryService.GetCountryById(countryId);
                if (country != null)
                {
                    location = country.GetLocalized(x => x.Name);
                }
                else
                {
                    locationEnabled = false;
                }
            }

            //private message
            bool pmEnabled = _forumSettings.AllowPrivateMessages && !user.IsGuest();

            //total forum posts
            bool totalPostsEnabled = false;
            int totalPosts = 0;
            if (_forumSettings.ForumsEnabled && _forumSettings.ShowUsersPostCount)
            {
                totalPostsEnabled = true;
                totalPosts = user.GetAttribute<int>(SystemUserAttributeNames.ForumPostCount);
            }

            //registration date
            bool joinDateEnabled = false;
            string joinDate = string.Empty;

            if (_userSettings.ShowUsersJoinDate)
            {
                joinDateEnabled = true;
                joinDate = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
            }

            //birth date
            bool dateOfBirthEnabled = false;
            string dateOfBirth = string.Empty;
            if (_userSettings.DateOfBirthEnabled)
            {
                var dob = user.GetAttribute<DateTime?>(SystemUserAttributeNames.DateOfBirth);
                if (dob.HasValue)
                {
                    dateOfBirthEnabled = true;
                    dateOfBirth = dob.Value.ToString("D");
                }
            }

            var model = new ProfileInfoModel()
            {
                UserProfileId = user.Id,
                AvatarEnabled = avatarEnabled,
                AvatarUrl = avatarUrl,
                LocationEnabled = locationEnabled,
                Location = location,
                PMEnabled = pmEnabled,
                TotalPostsEnabled = totalPostsEnabled,
                TotalPosts = totalPosts.ToString(),
                JoinDateEnabled = joinDateEnabled,
                JoinDate = joinDate,
                DateOfBirthEnabled = dateOfBirthEnabled,
                DateOfBirth = dateOfBirth,
            };

            return PartialView(model);
        }

        //latest posts tab
        [ChildActionOnly]
        public virtual ActionResult Posts(int userProfileId, int page)
        {
            var user = _userService.GetUserById(userProfileId);
            if (user == null)
            {
                return RedirectToRoute("HomePage");
            }

            if (page > 0)
            {
                page -= 1;
            }

            var pageSize = _forumSettings.LatestUserPostsPageSize;

            var list = _forumService.GetAllPosts(0, user.Id, string.Empty, false, page, pageSize);

            var latestPosts = new List<PostsModel>();

            foreach (var forumPost in list)
            {
                var posted = string.Empty;
                if (_forumSettings.RelativeDateTimeFormattingEnabled)
                {
                    posted = forumPost.CreatedOnUtc.RelativeFormat(true, "f");
                }
                else
                {
                    posted = _dateTimeHelper.ConvertToUserTime(forumPost.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
                }

                latestPosts.Add(new PostsModel()
                {
                    ForumTopicId = forumPost.TopicId,
                    ForumTopicTitle = forumPost.ForumTopic.Subject,
                    ForumTopicSlug = forumPost.ForumTopic.GetSeName(),
                    ForumPostText = forumPost.FormatPostText(),
                    Posted = posted
                });
            }

            var pagerModel = new PagerModel()
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "UserProfilePaged",
                UseRouteLinks = true,
                RouteValues = new RouteValues { page = page, id = userProfileId }
            };

            var model = new ProfilePostsModel()
            {
                PagerModel = pagerModel,
                Posts = latestPosts,
            };

            return PartialView(model);
        }
    }
}
