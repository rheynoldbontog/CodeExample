using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;

namespace SSG.Web.Infrastructure.Injecter
{
    public class IgnoreProperties : LoopValueInjection
    {
        private readonly IEnumerable<string> _ignores;

        public IgnoreProperties(params string[] i)
        {
            _ignores = i;
        }

        protected override bool UseSourceProp(string sourcePropName)
        {
            return !_ignores.Contains(sourcePropName);
        }
    }
}