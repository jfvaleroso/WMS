using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// This is a custom ckeditor extension
    /// </summary>
    public static class CKEditor
    {

        #region ckeditor
        public static MvcHtmlString CkEditor(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes)
        {
            var output = htmlHelper.TextArea(name, value, htmlAttributes).ToString();
           // output += string.Format("<script type=\"text/javascript\">$(document).ready(function(){{ $('#{0}').ckeditor(); }});</script>", name);

            return MvcHtmlString.Create(output);
        }
        public static MvcHtmlString CkEditor(this HtmlHelper htmlHelper, string name, string value)
        {
            return htmlHelper.CkEditor(name, value, null);
        }
        public static MvcHtmlString CkEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.CkEditor(metadata.PropertyName, metadata.Model as string, htmlAttributes);
        }
        public static MvcHtmlString CkEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            return htmlHelper.CkEditorFor(expression, null);
        }
        #endregion
    }
}