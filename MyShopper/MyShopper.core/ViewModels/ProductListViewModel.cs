using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShopper.core.Models;
namespace MyShopper.core.ViewModels
{
    public class ProductListViewModel
    {
    public IEnumerable<Product> products { get; set; }
    public IEnumerable<ProductCategory> productCategories { get; set; }
}
}
