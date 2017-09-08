using FluentValidation;
using SSG.Core.Domain.Users;
using SSG.Services.Localization;
using SSG.Web.Models.User;

namespace SSG.Web.Validators.User
{
    public class UserInfoValidator : AbstractValidator<UserInfoModel>
    {
        public UserInfoValidator(ILocalizationService localizationService, UserSettings userSettings)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.FirstName.Required"));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.LastName.Required"));

            //form fields
            RuleFor(x => x.Company).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Company.Required"))
                .When(x => userSettings.CompanyRequired && userSettings.CompanyEnabled);
            RuleFor(x => x.StreetAddress).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.StreetAddress.Required"))
                .When(x => userSettings.StreetAddressRequired && userSettings.StreetAddressEnabled);
            RuleFor(x => x.StreetAddress2).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.StreetAddress2.Required"))
                .When(x => userSettings.StreetAddress2Required && userSettings.StreetAddress2Enabled);
            RuleFor(x => x.ZipPostalCode).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.ZipPostalCode.Required"))
                .When(x => userSettings.ZipPostalCodeRequired && userSettings.ZipPostalCodeEnabled);
            RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.City.Required"))
                .When(x => userSettings.CityRequired && userSettings.CityEnabled);
            RuleFor(x => x.Phone).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Phone.Required"))
                .When(x => userSettings.PhoneRequired && userSettings.PhoneEnabled);
            RuleFor(x => x.Fax).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Fax.Required"))
                .When(x => userSettings.FaxRequired && userSettings.FaxEnabled);

            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("'Department' is required")
                .When(x => userSettings.DepartmentEnabled);
        }}
}