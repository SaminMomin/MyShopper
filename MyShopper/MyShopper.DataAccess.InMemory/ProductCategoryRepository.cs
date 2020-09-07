using MyShopper.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShopper.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productsCategory = new List<ProductCategory>();

        public ProductCategoryRepository()
        {
            productsCategory = cache["productsCategory"] as List<ProductCategory>;
            if (productsCategory == null)
            {
                productsCategory = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productsCategory"] = productsCategory;
        }

        public void Insert(ProductCategory p)
        {
            productsCategory.Add(p);
        }

        public void Update(ProductCategory product)
        {
            ProductCategory productCategoryToUpdate = productsCategory.Find(p => p.ID == product.ID);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = product;
            }
            else
            {
                throw new Exception("Product Category not found.");
            }
        }

        public ProductCategory Find(string ID)
        {
            ProductCategory productCategoryToFind = productsCategory.Find(p => p.ID == ID);
            if (productCategoryToFind != null)
            {
                return productCategoryToFind;
            }
            else
            {
                throw new Exception("Product Category not found.");
            }
        }
        public IQueryable<ProductCategory> Collection()
        {
            return productsCategory.AsQueryable();
        }

        public void Delete(String ID)
        {
            ProductCategory productCategoryToDelete = productsCategory.Find(p => p.ID == ID);
            if (productCategoryToDelete != null)
            {
                productsCategory.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product Category not found.");
            }
        }
    }
}
