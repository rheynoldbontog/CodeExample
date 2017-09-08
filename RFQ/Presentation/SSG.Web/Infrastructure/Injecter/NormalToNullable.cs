using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;

namespace SSG.Web.Infrastructure.Injector
{
    public class NormalToNullable : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            //ignore int = 0 and DateTime = to 1/01/0001
            if (c.SourceProp.Type == typeof(DateTime) && (DateTime)c.SourceProp.Value == default(DateTime) ||
                (c.SourceProp.Type == typeof(int) && (int)c.SourceProp.Value == default(int)))
                return false;

            return (c.SourceProp.Name == c.TargetProp.Name &&
                c.SourceProp.Type == Nullable.GetUnderlyingType(c.TargetProp.Type));
        }
    }
}