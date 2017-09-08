using FluentValidation;
using SSG.Services.Localization;
using SSG.Web.Models.User;

namespace SSG.Web.Validators.User
{
    public class PasswordRecoveryValidator : AbstractValidator<PasswordRecoveryModel>
    {
        public PasswordRecoveryValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.PasswordRecovery.Email.Required"));
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage(localizationService.GetResource("Account.PasswordRecovery.EmployeeID.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
        }}
}