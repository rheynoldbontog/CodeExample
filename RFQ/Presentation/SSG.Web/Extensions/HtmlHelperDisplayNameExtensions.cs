using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace SSG.Web.Extensions
{
    public static class HtmlHelperDisplayNameExtensions
    {
        /// <summary>
        /// Get the Display Name for a model.
        /// </summary>
        /// <typeparam name="TModel">The model's Type</typeparam>
        /// <param name="helper">The HTML Helper for the model.</param>
        /// <returns>Returns the DisplayName for the model.</returns>
        /// <remarks>
        /// Use the DisplayNameAttribute on your model to assign the name to be returned.
        /// Unfortunately we can not use the better/newer DisplayAttribute to specify the name
        /// because Microsoft marked it to only be used on properties, not on classes.
        /// 
        /// Use: @Html.DisplayNameFor()
        /// </remarks>
        public static MvcHtmlString DisplayNameFor<TModel>(this HtmlHelper<IEnumerable<TModel>> helper)
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(() => Activator.CreateInstance<TModel>(), typeof(TModel));
            return new MvcHtmlString(metadata.DisplayName ?? typeof(TModel).Name);
        }

        /// <summary>
        /// Gets the Display Name for a property of a type being handled by an IEnumerable.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper">Html helper for the IEnumerable&lt;TClass&gt;</param>
        /// <param name="model">An instance of an IEnumerable. Its generic parameter is the type we want the display name for.</param>
        /// <param name="expression">An expression indicating the property to get the display name for.</param>
        /// <returns>The display name for the property, or the property name if no display name is provided.</returns>
        /// <remarks>
        /// Use the DisplayAttribute to set the display name for the property.
        /// This is designed to be used in a View that takes an IEnumerable model.
        /// 
        /// View: @model = IEnumerable&lt;SomeClass&gt;
        /// Use: @Html.DisplayNameFor(Model, someclass => someclass.SomeProperty)
        /// </remarks>
        public static MvcHtmlString DisplayNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, IEnumerable<TClass> model, Expression<Func<TClass, TProperty>> expression)
        {
            var name = GetFullNameFromExpression(helper, expression);
            return GetDisplayNameForProperty<TClass>(name);
        }

        /// <summary>
        /// Gets the Display Name for a property of a type being handled by an IEnumerable.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper">Html helper for the IEnumerable&lt;T&gt;</param>
        /// <param name="expression">An expression indicating the property to get the display name for.</param>
        /// <returns>The display name for the property, or the property name if no display name is provided.</returns>
        /// <remarks>
        /// Use the DisplayAttribute to set the display name for the property.
        /// This is designed to be used in a View that takes an IEnumerable model.
        /// 
        /// View: @model = IEnumerable&lt;SomeClass&gt;
        /// Use: @Html.DisplayNameFor(someclass => someclass.SomeProperty)
        /// </remarks>
        public static MvcHtmlString DisplayNameFor<TModel, TProperty>(this HtmlHelper<IEnumerable<TModel>> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var name = GetFullNameFromExpression(helper, expression);
            return GetDisplayNameForProperty<TModel>(name);
        }

        private static string GetFullNameFromExpression<TModel, TClass, TProperty>(HtmlHelper<TModel> helper, Expression<Func<TClass, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return name;
        }

        private static MvcHtmlString GetDisplayNameForProperty<TClass>(string propertyName)
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(() => Activator.CreateInstance<TClass>(), typeof(TClass), propertyName);
            return new MvcHtmlString(metadata.DisplayName ?? typeof(TClass).Name);
        }
    }
}