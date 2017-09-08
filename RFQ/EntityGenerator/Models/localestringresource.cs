using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class localestringresource
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
        public virtual language language { get; set; }
    }
}
