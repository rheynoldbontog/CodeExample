﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SSG.Web.Models.RFQ;

namespace SSG.Web.Validators.RFQ
{
    public class RFQValidator : AbstractValidator<RFQModel>
    {
        public RFQValidator()
        {
            RuleFor(x => x.RequesterId).NotEmpty().WithMessage("'Requester' is required!");
            RuleFor(x => x.Department).NotEmpty().WithMessage("'Department' is required!");
            RuleFor(x => x.BuyerId).NotEmpty().WithMessage("'Buyer' is required!");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("'Subject' is required!")
                .Length(1, 200).WithMessage("'Subject' must be between 1 and 200 characters.");
        }
    }
}