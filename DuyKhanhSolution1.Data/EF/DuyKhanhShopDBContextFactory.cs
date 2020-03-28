using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuyKhanhSolution1.Data.EF
{
    public class DuyKhanhShopDBContextFactory : IDesignTimeDbContextFactory<DuyKhanhShopDBContext>
    {
        public DuyKhanhShopDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DuyKhanhSolutionDb");
            var optionsBuilder = new DbContextOptionsBuilder<DuyKhanhShopDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DuyKhanhShopDBContext(optionsBuilder.Options);
        }
    }
}
