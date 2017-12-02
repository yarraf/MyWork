using System.Web;
using System.Web.Optimization;

namespace Ratp.Hidalgo.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/datepickerjs").Include(
                     "~/Scripts/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                       "~/Scripts/datatables.js",
                       "~/Scripts/dataTables.fixedColumns.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/customescript").Include(
                "~/Scripts/myscript.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
               "~/Scripts/knockout-{version}.js",
               "~/Scripts/knockout.validation.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/navcss").Include(
                     "~/Content/pe-icon-7-stroke.css",
                     "~/Content/ct-navbar.css"));

            bundles.Add(new StyleBundle("~/Content/datepickercss").Include(
               "~/Content/ui/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
             "~/Content/datatables.css",
             "~/Content/fixedColumns.bootstrap.css"));

            //bundles.Add(new StyleBundle("~/Content/datepickercss").Include(
            //    "~/Content/ui/all.css", "~/Content/ui/all.css", "~/Content/ui/core.css", "~/Content/ui/accordion.css",
            //    "~/Content/ui/autocomplete.css", "~/Content/ui/button.css", "~/Content/ui/datepicker.css", "~/Content/ui/dialog.css",
            //    "~/Content/ui/draggable.css", "~/Content/ui/menu.css", "~/Content/ui/progressbar.css", "~/Content/ui/resizable.css",
            //    "~/Content/ui/selectable.css", "~/Content/ui/selectmenu.css", "~/Content/ui/sortable.css", "~/Content/ui/slider.css",
            //    "~/Content/ui/spinner.css", "~/Content/ui/tabs.css", "~/Content/ui/tooltip.css"));
        }
    }
}
