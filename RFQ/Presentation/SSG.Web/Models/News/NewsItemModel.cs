﻿using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using SSG.Web.Framework.Mvc;
using SSG.Web.Validators.News;

namespace SSG.Web.Models.News
{
    [Validator(typeof(NewsItemValidator))]
    public partial class NewsItemModel : BaseSSGEntityModel
    {
        public NewsItemModel()
        {
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
        }

        public string SeName { get; set; }

        public string Title { get; set; }

        public string Short { get; set; }

        public string Full { get; set; }

        public bool AllowComments { get; set; }

        public int NumberOfComments { get; set; }

        public DateTime CreatedOn { get; set; }

        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }
    }
}