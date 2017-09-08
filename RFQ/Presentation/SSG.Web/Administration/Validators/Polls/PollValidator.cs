using FluentValidation;
using SSG.Admin.Models.Polls;
using SSG.Services.Localization;

namespace SSG.Admin.Validators.Polls
{
    public class PollValidator : AbstractValidator<PollModel>
    {
        public PollValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Polls.Fields.Name.Required"));
        }
    }
}