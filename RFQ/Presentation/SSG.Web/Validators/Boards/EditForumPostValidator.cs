using FluentValidation;
using SSG.Services.Localization;
using SSG.Web.Models.Boards;

namespace SSG.Web.Validators.Boards
{
    public class EditForumPostValidator : AbstractValidator<EditForumPostModel>
    {
        public EditForumPostValidator(ILocalizationService localizationService)
        {            
            RuleFor(x => x.Text).NotEmpty().WithMessage(localizationService.GetResource("Forum.TextCannotBeEmpty"));
        }
    }
}