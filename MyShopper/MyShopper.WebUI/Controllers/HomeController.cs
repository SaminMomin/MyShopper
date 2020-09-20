using MyShopper.core;
using MyShopper.core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopper.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoryContext;

        public HomeController(IRepository<Product> productContext,IRepository<ProductCategory> productCategoryContext)
        {
            this.productContext = productContext;
            this.productCategoryContext = productCategoryContext;
        }
        public ActionResult Index()
        {
            List<Product> products = productContext.Collection().ToList();
            return View(products);
        }

        public ActionResult Details(string id)
        {
            Product product = productContext.Find(id);
            if (product != null)
            {
                return View(product);
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}