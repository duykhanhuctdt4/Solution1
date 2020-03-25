using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhSolution1.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreate { get; set; }

        public string SeoAlias { get; set; }

    }
}
