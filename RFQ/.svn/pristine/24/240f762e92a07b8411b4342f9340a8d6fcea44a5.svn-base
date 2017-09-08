using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class pollanswer
    {
        public pollanswer()
        {
            this.usercontents = new List<usercontent>();
        }

        public int Id { get; set; }
        public int PollId { get; set; }
        public string Name { get; set; }
        public int NumberOfVotes { get; set; }
        public int DisplayOrder { get; set; }
        public virtual poll poll { get; set; }
        public virtual ICollection<usercontent> usercontents { get; set; }
    }
}
