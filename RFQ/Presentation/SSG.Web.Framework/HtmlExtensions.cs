﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using SSG.Core;
using SSG.Core.Infrastructure;
using SSG.Services.Localization;
using SSG.Web.Framework.Localization;
using SSG.Web.Framework.Mvc;
using Telerik.Web.Mvc.UI;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SSG.Web.Framework
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString ResolveUrl(this HtmlHelper htmlHelper, string url)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return MvcHtmlString.Create(urlHelper.Content(url));
        }

        public static MvcHtmlString Hint(this HtmlHelper helper, string value)
        {
            // Create tag builder
            var builder = new TagBuilder("img");

            // Add attributes
            builder.MergeAttribute("src", ResolveUrl(helper, "~/Administration/Content/images/ico-help.gif").ToHtmlString());
            builder.MergeAttribute("alt", value);
            builder.MergeAttribute("title", value);

            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static HelperResult LocalizedEditor<T, TLocalizedModelLocal>(this HtmlHelper<T> helper, string name,
             Func<int, HelperResult> localizedTemplate,
             Func<T, HelperResult> standardTemplate)
            where T : ILocalizedModel<TLocalizedModelLocal>
            where TLocalizedModelLocal : ILocalizedModelLocal
        {
            return new HelperResult(writer =>
            {
                if (helper.ViewData.Model.Locales.Count > 1)
                {
                    var tabStrip = helper.Telerik().TabStrip().Name(name).Items(x =>
                    {
                        x.Add().Text("Standard").Content(standardTemplate(helper.ViewData.Model).ToHtmlString()).Selected(true);
                        for (int i = 0; i < helper.ViewData.Model.Locales.Count; i++)
                        {
                            var locale = helper.ViewData.Model.Locales[i];
                            var language = EngineContext.Current.Resolve<ILanguageService>().GetLanguageById(locale.LanguageId);
                            x.Add().Text(language.Name)
                                .Content(localizedTemplate
                                    (i).
                                    ToHtmlString
                                    ())
                                .ImageUrl("~/Content/images/flags/" + language.FlagImageFileName);
                        }
                    }).ToHtmlString();
                    writer.Write(tabStrip);
                }
                else
                {
                    standardTemplate(helper.ViewData.Model).WriteTo(writer);
                }
            });
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector = null) where T : BaseSSGEntityModel
        {
            return DeleteConfirmation<T>(helper, "", buttonsSelector);
        }

        // Adds an action name parameter for using other delete action names
        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName, string buttonsSelector = null) where T : BaseSSGEntityModel
        {
            if (String.IsNullOrEmpty(actionName))
                actionName = "Delete";

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation").ToHtmlString();

            //there's an issue in Telerik (ScriptRegistrar.Current implemenation)
            //it's a little hack to ensure ScriptRegistrar.Current is loaded
            helper.Telerik();

            #region Write click events for button, if supplied

            if (!string.IsNullOrEmpty(buttonsSelector))
            {
                var textWriter = new StringWriter();
                IClientSideObjectWriter objectWriter = new ClientSideObjectWriterFactory().Create(buttonsSelector, "click", textWriter);
                objectWriter.Start();
                textWriter.Write("function(e){e.preventDefault();openModalWindow(\"" + modalId + "\");}");
                objectWriter.Complete();
                var value = textWriter.ToString();
                ScriptRegistrar.Current.OnDocumentReadyStatements.Add(value);
            }

            #endregion

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = helper.ViewData.Model.Id,
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName
            };

            var window = helper.Telerik().Window().Name(modalId)
                .Title(EngineContext.Current.Resolve<ILocalizationService>().GetResource("Admin.Common.AreYouSure"))
                .Modal(true)
                .Effects(x => x.Toggle())
                .Resizable(x => x.Enabled(false))
                .Buttons(x => x.Close())
                .Visible(false)
                .Content(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString()).ToHtmlString();

            return MvcHtmlString.Create(window);
        }

        public static MvcHtmlString SSGLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool displayHint = true)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var hintResource = string.Empty;
            object value = null;
            if (metadata.AdditionalValues.TryGetValue("IOTechResourceDisplayName", out value))
            {
                var resourceDisplayName = value as SSGResourceDisplayName;
                if (resourceDisplayName != null && displayHint)
                {
                    var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                    hintResource =
                        EngineContext.Current.Resolve<ILocalizationService>()
                        .GetResource(resourceDisplayName.ResourceKey + ".Hint", langId);

                    result.Append(helper.Hint(hintResource).ToHtmlString());
                }
            }
            result.Append(helper.LabelFor(expression, new { title = hintResource }));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool useControlLabel = true)
        {
            return LabelFor(html, expression, new Dictionary<string, object>() {});
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            if (htmlAttributes.Count == 0)
                tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.AddCssClass("control-label");
            tag.SetInnerText(labelText + ":");
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString SearchLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool useControlLabel = true)
        {
            return SearchLabelFor(html, expression, new Dictionary<string, object>() { });
        }

        public static MvcHtmlString SearchLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return SearchLabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString SearchLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            if (htmlAttributes.Count == 0)
                tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.AddCssClass("search-control-label");
            tag.SetInnerText(labelText + ":");
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString RequiredHint(this HtmlHelper helper, string additionalText = null)
        {
            // Create tag builder
            var builder = new TagBuilder("span");
            builder.AddCssClass("required");
            var innerText = "*";
            //add additinal text if specified
            builder.Attributes.Add("title", "Required");
            builder.Attributes.Add("padding-top", "5px");
            if (!String.IsNullOrEmpty(additionalText))
                innerText += " " + additionalText;
            builder.SetInnerText(innerText);
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString RequiredHint(this HtmlHelper helper)
        {
            // Create tag builder
            var builder = new TagBuilder("span");
            builder.AddCssClass("required");
            var innerText = "*";
            builder.Attributes.Add("title", "Required");
            builder.Attributes.Add("padding-top", "5px");
            builder.SetInnerText(innerText);
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }

        /// <summary>
        /// Creates a days, months, years drop down list using an HTML select control. 
        /// The parameters represent the value of the "name" attribute on the select control.
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="dayName">"Name" attribute of the day drop down list.</param>
        /// <param name="monthName">"Name" attribute of the month drop down list.</param>
        /// <param name="yearName">"Name" attribute of the year drop down list.</param>
        /// <param name="beginYear">Begin year</param>
        /// <param name="endYear">End year</param>
        /// <param name="selectedDay">Selected day</param>
        /// <param name="selectedMonth">Selected month</param>
        /// <param name="selectedYear">Selected year</param>
        /// <param name="localizeLabels">Localize labels</param>
        /// <returns></returns>
        public static MvcHtmlString DatePickerDropDowns(this HtmlHelper html,
            string dayName, string monthName, string yearName,
            int? beginYear = null, int? endYear = null,
            int? selectedDay = null, int? selectedMonth = null, int? selectedYear = null, bool localizeLabels = true)
        {
            var daysList = new TagBuilder("select");
            var monthsList = new TagBuilder("select");
            var yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", dayName);
            monthsList.Attributes.Add("name", monthName);
            yearsList.Attributes.Add("name", yearName);

            var days = new StringBuilder();
            var months = new StringBuilder();
            var years = new StringBuilder();

            string dayLocale, monthLocale, yearLocale;
            if (localizeLabels)
            {
                var locService = EngineContext.Current.Resolve<ILocalizationService>();
                dayLocale = locService.GetResource("Common.Day");
                monthLocale = locService.GetResource("Common.Month");
                yearLocale = locService.GetResource("Common.Year");
            }
            else
            {
                dayLocale = "Day";
                monthLocale = "Month";
                yearLocale = "Year";
            }

            days.AppendFormat("<option value='{0}'>{1}</option>", "0", dayLocale);
            for (int i = 1; i <= 31; i++)
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedDay.HasValue && selectedDay.Value == i) ? " selected=\"selected\"" : null);


            months.AppendFormat("<option value='{0}'>{1}</option>", "0", monthLocale);
            for (int i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>",
                                    i, 
                                    (selectedMonth.HasValue && selectedMonth.Value == i) ? " selected=\"selected\"" : null,
                                    CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }


            years.AppendFormat("<option value='{0}'>{1}</option>", "0", yearLocale);

            if (beginYear == null)
                beginYear = DateTime.UtcNow.Year - 100;
            if (endYear == null)
                endYear = DateTime.UtcNow.Year;

            for (int i = beginYear.Value; i <= endYear.Value; i++)
                years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);

            daysList.InnerHtml = days.ToString();
            monthsList.InnerHtml = months.ToString();
            yearsList.InnerHtml = years.ToString();

            return MvcHtmlString.Create(string.Concat(daysList, monthsList, yearsList));
        }


        public static MvcHtmlString Widget(this HtmlHelper helper, string widgetZone)
        {
            return helper.Action("WidgetsByZone", "Widget", new { widgetZone = widgetZone });
        }

        public static MvcForm BeginHorizontalForm(this HtmlHelper helper)
        {
            return helper.BeginForm(null, null, FormMethod.Post, new { @class = "form-horizontal" });
        }

        public static MvcForm BeginHorizontalForm(this HtmlHelper helper, string formId)
        {
            return helper.BeginForm(null, null, FormMethod.Post, new { @class = "form-horizontal", id = formId });
        }

        public static MvcForm BeginHorizontalForm(this HtmlHelper helper, object htmlAttributes)
        {
            return helper.BeginForm(null, null, FormMethod.Post, htmlAttributes);
        }

        public static MvcForm BeginRouteHorizontalForm(this HtmlHelper helper, string routeName, FormMethod method)
        {
            return helper.BeginRouteForm(routeName, method, new { @class = "form-horizontal" });
        }

        public static MvcForm BeginRouteHorizontalForm(this HtmlHelper helper, string routeName, FormMethod method, object htmlAttributes)
        {
            return helper.BeginRouteForm(routeName, method, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> modelExpression, string firstElement)
        {
            var typeOfProperty = modelExpression.ReturnType;
            if (!typeOfProperty.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", typeOfProperty));

            var enumValues = new SelectList(Enum.GetValues(typeOfProperty));
            return htmlHelper.DropDownListFor(modelExpression, enumValues, firstElement);
        }
    }
}
