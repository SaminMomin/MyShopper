﻿using MyShopper.core.Contracts;
using MyShopper.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopper.WebUI.Controllers
{
    public class OrderManagerController : Controller
    {
        // GET: OrderManager
        IOrderService orderService;
        
        public OrderManagerController (IOrderService orderService)
        {
            this.orderService = orderService;
        }
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            return View(orders);
        }

        public ActionResult UpdateOrder(string Id)
        {
            ViewBag.StatusList = new List<string>() { "Order Created","Payment Processed","Order Shipped","Order Delivered"};
            Order order = orderService.GetOrder(Id);
            return View(order);
        }
        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string Id)
        {
            Order order = orderService.GetOrder(Id);
            order.OrderStatus = updatedOrder.OrderStatus;
            orderService.UpdateOrder(order);
            return RedirectToAction("Index");
        }
    }
}