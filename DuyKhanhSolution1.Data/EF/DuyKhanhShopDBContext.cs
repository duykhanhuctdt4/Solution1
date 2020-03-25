using DuyKhanhSolution1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhSolution1.Data.EF
{
    public class DuyKhanhShopDBContext : DbContext
    {
        public DuyKhanhShopDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products;
        public DbSet<Category> Categories;

    }
}
