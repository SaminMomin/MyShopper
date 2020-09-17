using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShopper.core;
using MyShopper.core.Contracts;
using MyShopper.core.ViewModels;
using MyShopper.DataAccess.InMemory;
namespace MyShopper.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoriesContext;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            this.productContext = productContext;
            this.productCategoriesContext= productCategoriesContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = productContext.Collection().ToList();
            return View(products);
        }
      
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.product=new Product();
            viewModel.productCategories = productCategoriesContext.Collection();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
               return View(product);
            }
            else
            {              
                productContext.Insert(product);
                productContext.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product productToEdit = productContext.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.product = productToEdit;
                viewModel.productCategories = productCategoriesContext.Collection();

                return View(viewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product,string Id)
        {
            Product productToEdit = productContext.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;
                    productContext.Commit();
                    return RedirectToAction("Index");                    
                }                
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = productContext.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = productContext.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                productContext.Delete(Id);
                productContext.Commit();
                return RedirectToAction("Index");
            }
        }

    }
}