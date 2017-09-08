using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class genericattribute
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string KeyGroup { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
