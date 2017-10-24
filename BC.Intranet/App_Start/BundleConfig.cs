using System.Web;
using System.Web.Optimization;

namespace BC.Intranet
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/frame/scripts").Include(
                        "~/frame/scripts/jquery-{version}.js",
                        "~/frame/scripts/jquery.validate*",
                        "~/frame/scripts/bootstrap.js",
                      "~/frame/scripts/respond.js"));

                        // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/frame/scripts/modernizr").Include(
                        "~/frame/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/frame/css").Include(
                      "~/frame/css/bootstrap.css",
                      "~/frame/css/site.css"));
        }
    }
}
