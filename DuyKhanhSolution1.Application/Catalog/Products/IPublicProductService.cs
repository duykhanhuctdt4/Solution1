using DuyKhanhSolution1.Application.Catalog.Products.Dtos;
using DuyKhanhSolution1.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhSolution1.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        PageViewModel<ProductViewModel> GetAllCaterygoryId(int categoryId, int pageIndex, int pageSize);
    }
}
