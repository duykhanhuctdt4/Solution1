using DuyKhanhShop.Ultilities.Exceptions;
using DuyKhanhSolution1.Application.Catalog.Products.Dtos;
using DuyKhanhSolution1.Application.Catalog.Products.Dtos.Manage;
using DuyKhanhSolution1.Application.Dtos;
using DuyKhanhSolution1.Data.EF;
using DuyKhanhSolution1.Data.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DuyKhanhSolution1.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly DuyKhanhShopDBContext _context;
        public ManageProductService(DuyKhanhShopDBContext context)
        {
            _context = context;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
            
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreate = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if(product == null)
                throw new DuyKhanhCustomExceptions($"Can not find a product: {productId}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public  async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            // 1. Select join
            var query = from p   in _context.Products
                        join pt  in _context.ProductTranslations    on p.Id              equals pt.ProductId
                        join pic in _context.ProductInCategories    on p.Id              equals pic.ProductId
                        join c   in _context.Categories             on pic.CategoryId    equals c.Id
                        select new { p, pt , pic };
            // 2.filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if(request.CategoryIds.Count >0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }
            //3. Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1)* request.PageSize).
                Take(request.PageSize)
                .Select(x=>new ProductViewModel()
                { 
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreate = x.p.DateCreate,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4. Select and project
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<int> Update(ProducUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, decimal addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
