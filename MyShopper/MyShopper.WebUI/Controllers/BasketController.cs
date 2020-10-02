using MyShopper.core.Contracts;
using MyShopper.core.Models;
using MyShopper.core.ViewModels;
using MyShopper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopper.WebUI.Controllers
{
    public class BasketController : Controller
    {
        public IBasketService basketService;
        public IOrderService orderService;
        public IRepository<Customer> customers;
        public BasketController(IBasketService BasketService,IOrderService orderService, IRepository<Customer> customers)
        {
            this.basketService = BasketService;
            this.orderService = orderService;
            this.customers = customers;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }
        
        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public PartialViewResult GetBasketSummary()
        {
            var model = basketService.GetBasketSummary(this.HttpContext);
            return PartialView(model);
        }
        [Authorize]
        public ActionResult CheckOut()
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            if (customer != null)
            {
                Order order = new Order()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    City = customer.City,
                    Street = customer.Street,
                    State = customer.State,
                    ZipCode = customer.ZipCode,
                    Email = customer.Email
                };
                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult CheckOut(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;
            //process payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou",new { OrderId =order.Id});
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}