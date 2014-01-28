using System.Web;
using System.Web.Optimization;

namespace WMS.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region CSS Style
            bundles.Add(new StyleBundle("~/Content/main/css").Include("~/Content/main/main.css").Include("~/Content/main/basic.css"));



            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include("~/Content/bootstrap/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/kendo/2012.2.913").Include("~/Content/kendo/2012.2.913/kendo.*"));

            bundles.Add(new StyleBundle("~/scripts/jtable/themes/metro/jeff/style").Include(
              "~/Scripts/jtable/themes/metro/jeff/jtable.css"));

            #endregion
            #region Script
            bundles.Add(new ScriptBundle("~/bundles/html5").Include("~/Scripts/html5/html5shiv.js")
               .Include("~/Scripts/html5/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/global").Include("~/Scripts/global/jquery.blockUI.js"));
            bundles.Add(new ScriptBundle("~/bundles/global/main")
                .Include("~/Scripts/global/main.js")
                .Include("~/Scripts/global/workflow.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tablednd").Include(
                     "~/Scripts/tablednd/jquery.tablednd.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap/bootstrap.min.js").Include("~/Scripts/bootstrap/bootstrap-select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                         "~/Scripts/jqueryui/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/Scripts/unobtrusive/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.validate").Include(
                          "~/Scripts/jquery.validate/jquery.validate.js").Include(
                          "~/Scripts/jquery.validate/jquery.validate.unobtrusive.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/kendoui/2012.2.913").Include(
                          "~/Scripts/kendoui/2012.2.913/kendo.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
                     "~/Scripts/jtable/jquery.jtable.js"));

            //mvc ajax
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/mvcAjax").Include
                      ("~/Scripts/mvc.ajax/MicrosoftAjax.js",
                      "~/Scripts/mvc.ajax/MicrosoftMvcAjax.js"));
            //ckeditor
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include
                     ("~/Scripts/ckeditor/ckeditor.js"));
            #endregion
           
        }
    }
}