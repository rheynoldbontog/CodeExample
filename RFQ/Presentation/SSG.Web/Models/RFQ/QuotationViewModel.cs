using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSG.Core.Domain.RFQ;

namespace SSG.Web.Models.RFQ
{
    public class QuotationViewModel
    {
        public RFQLineIndexModel RfqLineIndexModel { get; set; }
        public FileAttachment FileAttachment { get; set; }
        public QuotationModel QuotationView { get; set; }
        public IEnumerable<QuotationModel> QuotationModel { get; set; }
    }
}