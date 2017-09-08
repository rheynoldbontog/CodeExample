using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SSG.Web.Models.RFQ;

namespace SSG.Web.Validators.RFQ
{
    public class ReassignBuyerValidator : AbstractValidator<ReassignBuyerModel>
    {
        public ReassignBuyerValidator()
        {
            RuleFor(x => x.BuyerId).NotEmpty().WithMessage("'Buyer' is required!");
            RuleFor(x => x.ChangeReason)
                .NotEmpty().WithMessage("'Change Reason' is required!")
                .Length(1, 500).WithMessage("'Change Reason' must be between 1 and 500 characters");
        }
    }
}