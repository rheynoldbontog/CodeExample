using FluentValidation;
using SSG.Admin.Models.Settings;
using SSG.Services.Localization;

namespace SSG.Admin.Validators.Settings
{
    public class SettingValidator : AbstractValidator<SettingModel>
    {
        public SettingValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.AllSettings.Fields.Name.Required"));
        }
    }
}