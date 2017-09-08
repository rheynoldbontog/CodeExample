using FluentValidation;
using SSG.Admin.Models.Users;
using SSG.Services.Localization;

namespace SSG.Admin.Validators.Users
{
    public class UserRoleValidator : AbstractValidator<UserRoleModel>
    {
        public UserRoleValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Admin.Users.UserRoles.Fields.Name.Required"));
        }
    }
}