using DuyKhanhSolution1.Application.Catalog.Products.Dtos;
using DuyKhanhSolution1.Application.Dtos;
using DuyKhanhSolution1.Data.EF;
using DuyKhanhSolution1.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DuyKhanhSolution1.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly DuyKhanhShopDBContext _context;
        public ManageProductService(DuyKhanhShopDBContext context)
        {
            _context = context;
        }
        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,

            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageViewModel<ProductViewModel>> GetAllPaging(string keyword, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductEditRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
