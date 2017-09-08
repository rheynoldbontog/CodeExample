CREATE INDEX `IX_LocaleStringResource` ON `LocaleStringResource` (`ResourceName` (200) ASC,  `LanguageId` ASC);

CREATE INDEX `IX_Country_DisplayOrder` ON `Country` (`DisplayOrder` ASC);

CREATE INDEX `IX_StateProvince_CountryId` ON `StateProvince` (`CountryId` ASC);

CREATE INDEX `IX_Currency_DisplayOrder` ON `Currency` ( `DisplayOrder` ASC);

CREATE INDEX `IX_Log_CreatedOnUtc` ON `Log` (`CreatedOnUtc` ASC);

CREATE INDEX `IX_User_UserGuid` ON `User` (`UserGuid` ASC);

CREATE INDEX `IX_QueuedEmail_CreatedOnUtc` ON `QueuedEmail` (`CreatedOnUtc` ASC);

CREATE INDEX `IX_Language_DisplayOrder` ON `Language` (`DisplayOrder` ASC);

CREATE INDEX `IX_News_LanguageId` ON `News` (`LanguageId` ASC);

CREATE INDEX `IX_NewsComment_NewsItemId` ON `NewsComment` (`NewsItemId` ASC);

CREATE INDEX `IX_PollAnswer_PollId` ON `PollAnswer` (`PollId` ASC);

CREATE INDEX `IX_Forums_Group_DisplayOrder` ON `ForumsGroup` (`DisplayOrder` ASC);

CREATE INDEX `IX_Forums_Forum_DisplayOrder` ON `ForumsForum` (`DisplayOrder` ASC);

CREATE INDEX `IX_Forums_Forum_ForumGroupId` ON `ForumsForum` (`ForumGroupId` ASC);

CREATE INDEX `IX_Forums_Topic_ForumId` ON `ForumsTopic` (`ForumId` ASC);

CREATE INDEX `IX_Forums_Post_TopicId` ON `ForumsPost` (`TopicId` ASC);

CREATE INDEX `IX_Forums_Post_UserId` ON `ForumsPost` (`UserId` ASC);

CREATE INDEX `IX_Forums_Subscription_ForumId` ON `ForumsSubscription` (`ForumId` ASC);

CREATE INDEX `IX_Forums_Subscription_TopicId` ON `ForumsSubscription` (`TopicId` ASC);
