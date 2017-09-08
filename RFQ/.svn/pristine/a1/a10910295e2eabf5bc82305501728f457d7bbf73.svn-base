using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SSG.Web.Models.RFQ;

namespace SSG.Web.Validators.RFQ
{
    public class QuotationValidator : AbstractValidator<QuotationModel>
    {
        public QuotationValidator()
        {
            RuleFor(x => x.Supplier).NotEmpty().WithMessage("Quotation {0}: Supplier is required!")
                                    .Length(1, 200).WithMessage("Quotation {0}: 'Supplier' must be between 1 and 200 characters");
            RuleFor(x => x.Commodity).NotEmpty().WithMessage("Quotation {0}: Commodity is required!");
            RuleFor(x => x.QuoteItemDescription).NotEmpty().WithMessage("Quotation {0}: Item Description is required!")
                                                .Length(1, 100).WithMessage("Quotation {0}: 'Quote Item Description' must be between 1 and 100 characters");

            RuleFor(x => x.DeliveryLeadTime).Length(0, 100).WithMessage("Quotation {0}: 'Delivery Lead Time' must be between 0 and 100 characters");
            RuleFor(x => x.PaymentTerms).Length(0, 100).WithMessage("Quotation {0}: 'Payment Terms' must be between 0 and 100 characters");
            RuleFor(x => x.WarrantyPeriod).Length(0, 50).WithMessage("Quotation {0}: 'Warranty Period' must be between 0 and 50 characters");

            RuleFor(x => x.CurrencyId).NotEmpty().WithMessage("Quotation {0}: Currency is required!");
            RuleFor(x => x.UnitPrice).NotNull().WithMessage("Quotation {0}: Unit Price is required!");

            RuleFor(x => x.BuyerRemarks).Length(0, 250).WithMessage("Quotation {0}: 'Buyer Remarks' must be between 0 and 250 characters");
            RuleFor(x => x.RequesterRemarks).Length(0, 250).WithMessage("Quotation {0}: 'Requester Remarks' must be between 0 and 250 characters");

            RuleFor(x => x.QuoteValidity ).Length(0, 250).WithMessage("'Quote Validity' must be between 0 and 100 characters");
        }
    }
} 