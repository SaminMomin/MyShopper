using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopper.core
{
    public class ProductCategory
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public ProductCategory()
        {
            this.ID = Guid.NewGuid().ToString();
        }

    }
}
