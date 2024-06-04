using System.Web.Optimization;

namespace WebProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS bundles
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/FrontEndAssets/CSS/main.css",
                "~/FrontEndAssets/owlcarousel/owl.carousel.min.css",
                "~/FrontEndAssets/owlcarousel/owl.theme.default.min.css"
            ));

            // JavaScript bundles
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/FrontEndAssets/Js/main.js",
                "~/FrontEndAssets/owlcarousel/owl.carousel.min.js"
            ));

            // Enable optimizations
            BundleTable.EnableOptimizations = true;
        }
    }
}