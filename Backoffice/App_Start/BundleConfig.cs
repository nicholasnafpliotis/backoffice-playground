using System.Web;
using System.Web.Optimization;

namespace  Backoffice
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/site").Include(
                "~/Content/scripts/jquery-ui.js",
                "~/Content/scripts/modernizr.js",
                "~/Content/scripts/knockout.js",
                "~/Content/scripts/bootstrap.js",
                "~/Content/scripts/custom.js",
                "~/Content/scripts/cookies.js"));

            bundles.Add(new StyleBundle("~/styles/site").Include(
                "~/Content/site.min.css"));

            // Enable bundling optimizations, even when the site is in debug mode.
            BundleTable.EnableOptimizations = true;
        }
    }
}