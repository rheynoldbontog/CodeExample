using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.Users;

namespace SSG.Core.Domain.RFQ
{
    public static class DepartmentExtension
    {
        public static User GetDefaultBuyer(this Department department)
        {
            if (department != null && department.Buyers != null && department.Buyers.Count() > 0)
                return department.Buyers.ElementAt(0);
            else
                return new User();
        }
    }
}
