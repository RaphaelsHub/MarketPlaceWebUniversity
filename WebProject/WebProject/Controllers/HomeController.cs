using System.Collections.Generic;
using System.Web.Mvc;
using WebProject.ModelAccessLayer.Model;




namespace WebProject.Controllers
{
    public class HomeController : BaseController
    {
        public static List<Category> AllCategories { get; private set; }

        //работает
        // GET: Home
        public ActionResult ThanksForOrder() => View();

        //работает
        public ActionResult Error() => View();

        //работает
        public ActionResult Error404() => View();

        //работает
        public ActionResult Index()
        {
            AllCategories = _businessLogic.User.GetCategoriesView();

            if(AllCategories == null)
                AllCategories = new List<Category>();

            return View(_businessLogic.User.GetAllProducts());
        }


        //работает
        public ActionResult Search(string text)
        {
            return (text == null) ? View() : View(_businessLogic.User.GetProductssByName(text));
        }
    }
}

