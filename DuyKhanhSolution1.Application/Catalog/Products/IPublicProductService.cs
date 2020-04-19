using DuyKhanhSolution1.Application.Catalog.Products.Dtos;
using DuyKhanhSolution1.Application.Catalog.Products.Dtos.Public;
using DuyKhanhSolution1.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhSolution1.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        PagedResult<ProductViewModel> GetAllCaterygoryId(GetProductPagingRequest request);
    }
}
