using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SSG.Web.Models.RFQ;
using SSG.Core.Domain.RFQ;

namespace SSG.Web.Validators.RFQ
{
    public class RFQLineValidator : AbstractValidator<RFQLineModel>
    {
        public RFQLineValidator()
        {
            RuleFor(x => x.RFQLineNo).NotEmpty().WithMessage("'RFQ Line No.' is required!");
            RuleFor(x => x.LineTypeId).NotEmpty().WithMessage("'RFQ Line Type' is required!");
            RuleFor(x => x.FormTypeId).NotEmpty().WithMessage("'RFQ Line Form Type' is required!");
            RuleFor(x => x.ItemDescription).NotEmpty().WithMessage("'Item Description' is required!")
                                           .Length(1, 200).WithMessage("'Item Description' must be between 1 and 200 characters.");
            RuleFor(x => x.Quantity).NotNull().WithMessage("'Quantity' is required!");
            RuleFor(x => x.QuantityUnitId).NotEmpty().WithMessage("'Unit' is required!");
            RuleFor(x => x.Maker).NotEmpty().WithMessage("'Maker' is required!")
                                 .Length(1, 50).WithMessage("'Maker' must be between 1 and 50 characters.");
            RuleFor(x => x.MakerPN).NotEmpty().WithMessage("'MakerPN' is required!")
                                   .Length(1, 50).WithMessage("'MakerPN' must be between 1 and 50 characters.");

            RuleFor(x => x.ROHSCompliant).NotNull().When(x => x.FormTypeId == (int)RFQLineFormTypes.GeneralForm).WithMessage("ROHS Compliance is required!");

            RuleFor(x => x.TestEquipmentApplication).Length(0, 100).WithMessage("'Test Equipment Application' must be between 0 and 50 characters");
            RuleFor(x => x.TestEquipmentPurchaseTypeId).NotEmpty().When(x => x.FormTypeId == (int)RFQLineFormTypes.TestEquipmentRentalAndCalibrationForm).WithMessage("Equipment Purchase Type is required!");
            RuleFor(x => x.TestEquipmentCalibrationTypeId).NotEmpty().When(x => x.FormTypeId == (int)RFQLineFormTypes.TestEquipmentRentalAndCalibrationForm).WithMessage("Equipment Calibration Type is required!");

            RuleFor(x => x.NoteToBuyer).Length(0, 250).WithMessage("'Note to Buyer' must be between 0 and 250 characters");
        }
    }
}