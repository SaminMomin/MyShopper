using MyShopper.core;
using MyShopper.core.Contracts;
using MyShopper.core.Models;
using MyShopper.core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShopper.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;
        public const string BasketSessionName = "eCommerce Shopper";
        public BasketService(IRepository<Product> Product,IRepository<Basket> Basket)
        {
            this.productContext = Product;
            this.basketContext = Basket;
        }
        private Basket Get(HttpContextBase httpContext,bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();
            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);
            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = Get(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                item = new BasketItem() { BasketId = basket.Id, ProductId=productId, quantity=1};
                basket.BasketItems.Add(item);
            }
            else
            {
                item.quantity = item.quantity + 1;
            }
            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext,string itemId)
        {
            Basket basket = Get(httpContext,true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            //createifnull is set to false as we don't want to create a new basket in case if it doesn't exist
            Basket basket = Get(httpContext, false);
            if (basket != null)
            {
                var result = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Productname = p.Name,
                                  Price = p.Price,
                                  Image = p.Image,
                                  Quantity = b.quantity

                              }).ToList();
                return result;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }

        }

        public BasketSummaryViewModel GetBasketSummary (HttpContextBase httpContext)
        {
            Basket basket = Get(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                int? basketcount = (from item in basket.BasketItems
                                    select item.quantity).Sum();
                decimal? baskettotal = (from item in basket.BasketItems
                                        join p in productContext.Collection()
                                        on item.ProductId equals p.Id
                                        select item.quantity * p.Price).Sum();
                model.BasketCount = basketcount??0;
                model.BasketTotal = baskettotal??decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }

        public void ClearBasket(HttpContextBase httpContext)
        {
            Basket basket = Get(httpContext, false);
            basket.BasketItems.Clear();
            basketContext.Commit();
        }
    }
}
