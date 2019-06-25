using System;
using System.Collections;
using System.Collections.Generic;

namespace Hello.Models
{
    public partial class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<SKU> SKUs { get; set; }
    }

    public partial class SKU
    {
        public string SkuName { get; set; }
        public decimal SuggestedRetailPrice { get; set; }
        public decimal WholesalePrice { get; set; }
    }
}
