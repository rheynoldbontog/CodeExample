using FluentValidation;
using SSG.Admin.Models.Users;
using SSG.Core.Domain.Users;
using SSG.Services.Localization;

namespace SSG.Admin.Validators.Users
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator(ILocalizationService localizationService, UserSettings userSettings)
        {
            //form fields
            RuleFor(x => x.Company).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.Company.Required"))
                .When(x => userSettings.CompanyRequired && userSettings.CompanyEnabled);
            RuleFor(x => x.StreetAddress).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.StreetAddress.Required"))
                .When(x => userSettings.StreetAddressRequired && userSettings.StreetAddressEnabled);
            RuleFor(x => x.StreetAddress2).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.StreetAddress2.Required"))
                .When(x => userSettings.StreetAddress2Required && userSettings.StreetAddress2Enabled);
            RuleFor(x => x.ZipPostalCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.ZipPostalCode.Required"))
                .When(x => userSettings.ZipPostalCodeRequired && userSettings.ZipPostalCodeEnabled);
            RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.City.Required"))
                .When(x => userSettings.CityRequired && userSettings.CityEnabled);
            RuleFor(x => x.Phone).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.Phone.Required"))
                .When(x => userSettings.PhoneRequired && userSettings.PhoneEnabled);
            RuleFor(x => x.Fax).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.Users.Fields.Fax.Required"))
                .When(x => userSettings.FaxRequired && userSettings.FaxEnabled);
        }
    }
}