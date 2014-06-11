using System.Web.Optimization;

namespace IIUSchoolSystem
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/MainScripts").Include("~/UserScripts/Generics.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/Application").Include(
                "~/Scripts/jquery-ui-1.10.3.min.js",
                //"~/Scripts/morris.js",
                "~/Scripts/bootstrap3-wysihtml5.all.min.js",
                "~/Scripts/icheck.js",
                "~/Scripts/app.js",
                 "~/Scripts/jquery.maskedinput.min.js",
                "~/Scripts/chosen.jquery.min.js",
                "~/Scripts/demo.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/font-awesome.css",
                "~/Content/css/ionicons.css",
                //"~/Content/css/morris.css",
                "~/Content/css/bootstrap3-wysihtml5.css",
                "~/Content/css/chosen.css",
                "~/Content/css/Site.css",
                "~/Content/css/jquery-ui-1.10.3.full.min.css",
                "~/Content/css/jquery-ui-1.10.3.custom.min.css"));

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
        }
    }
}