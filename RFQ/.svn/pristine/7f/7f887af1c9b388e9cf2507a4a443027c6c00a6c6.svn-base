using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class poll
    {
        public poll()
        {
            this.pollanswers = new List<pollanswer>();
        }

        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string SystemKeyword { get; set; }
        public bool Published { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool AllowGuestsToVote { get; set; }
        public int DisplayOrder { get; set; }
        public Nullable<System.DateTime> StartDateUtc { get; set; }
        public Nullable<System.DateTime> EndDateUtc { get; set; }
        public virtual language language { get; set; }
        public virtual ICollection<pollanswer> pollanswers { get; set; }
    }
}
