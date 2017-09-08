﻿using System;
using System.Text;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Infrastructure;

namespace SSG.Web.Extensions
{
    public static class EditorExtensions
    {
        public static MvcHtmlString BBCodeEditor<TModel>(this HtmlHelper<TModel> html, string name)
        {
            var sb = new StringBuilder();
            
            var siteLocation = EngineContext.Current.Resolve<IWebHelper>().GetSiteLocation();
            string bbEditorWebRoot = String.Format("{0}Content/", siteLocation);

            sb.AppendFormat("<script src=\"{0}Content/editors/BBEditor/ed.js\" type=\"text/javascript\"></script>", siteLocation);
            sb.Append(Environment.NewLine);
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
            sb.Append(Environment.NewLine);
            sb.AppendFormat("    var webRoot = '{0}';", bbEditorWebRoot);
            sb.Append(Environment.NewLine);
            sb.AppendFormat("    edToolbar('{0}');", name);
            sb.Append(Environment.NewLine);
            sb.Append("</script>");
            sb.Append(Environment.NewLine);

            return MvcHtmlString.Create(sb.ToString());
        }


    }
}