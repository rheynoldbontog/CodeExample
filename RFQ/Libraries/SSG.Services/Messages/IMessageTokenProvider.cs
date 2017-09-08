﻿using System.Collections.Generic;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Messages;
using SSG.Core.Domain.News;
using SSG.Core.Domain.RFQ;
using SSG.Core.Domain.Users;

namespace SSG.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        void AddSiteTokens(IList<Token> tokens);

        void AddUserTokens(IList<Token> tokens, User user);

        void AddNewsLetterSubscriptionTokens(IList<Token> tokens, NewsLetterSubscription subscription);

        void AddNewsCommentTokens(IList<Token> tokens, NewsComment newsComment);

        void AddForumTokens(IList<Token> tokens, Forum forum);

        void AddForumTopicTokens(IList<Token> tokens, ForumTopic forumTopic,
            int? friendlyForumTopicPageIndex = null, int? appendedPostIdentifierAnchor = null);

        void AddForumPostTokens(IList<Token> tokens, ForumPost forumPost);

        void AddPrivateMessageTokens(IList<Token> tokens, PrivateMessage privateMessage);

        string[] GetListOfCampaignAllowedTokens();

        string[] GetListOfAllowedTokens();

        void AddRFQTokens(IList<Token> tokens, SSG.Core.Domain.RFQ.RFQ rfq);

        void AddRFQLineTokens(IList<Token> tokens, SSG.Core.Domain.RFQ.RFQLine rfqLine);

        void AddOpenRFQReportTokens(IList<Token> tokens);

    }
}