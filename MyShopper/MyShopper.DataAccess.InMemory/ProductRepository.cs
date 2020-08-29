using MyShopper.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShopper.DataAccess.InMemory
{
    class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products=new List<Product>();

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.ID == product.ID);
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public Product Find(string ID)
        {
            Product productToFind= products.Find(p => p.ID == ID);
            if (productToFind!= null)
            {
                return productToFind;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }
        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(String ID)
        {
            Product productToDelete = products.Find(p => p.ID == ID);
            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }
    }
}
