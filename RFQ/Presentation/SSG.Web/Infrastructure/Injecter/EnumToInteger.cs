using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;

namespace SSG.Web.Infrastructure.Injector
{
    public class EnumToInteger : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return (c.SourceProp.Name == c.TargetProp.Name &&
                c.SourceProp.Type.IsSubclassOf(typeof(Enum)) &&
                (c.TargetProp.Type == typeof(int) ||                                            // Non-nullable int
                (c.TargetProp.Type == typeof(int?) && c.Source.Value != null)));                // Nullable int
        }
    }
}