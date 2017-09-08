﻿using FluentValidation;
using SSG.Admin.Models.Forums;
using SSG.Services.Localization;

namespace SSG.Admin.Validators.Forums
{
    public class ForumGroupValidator : AbstractValidator<ForumGroupModel>
    {
        public ForumGroupValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Fields.Name.Required"));
        }
    }
}