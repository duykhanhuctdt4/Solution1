using DuyKhanhSolution1.Data.Entities;
using DuyKhanhSolution1.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhSolution1.Data.Extensions
{
    public static class MoldelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
            new AppConfig { Key = "HomeTitle", Value = "Config1" },
            new AppConfig { Key = "HomeKeyword", Value = "Config2" },
            new AppConfig { Key = "HomeDescription", Value = "Config3" }
        );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tieng Viet", IsDefault = true },
                new Language() { Id = "en-US", Name = "English", IsDefault = false }
                );
            modelBuilder.Entity<Category>().HasData(
            new Category ()
            {
                Id = 1,
                IsShowHome = true,
                ParentId = null,
                SortOrder = 1,
                Status = Status.Active,
            },
            new Category()
            {
                Id = 2,
                IsShowHome = true,
                ParentId = null,
                SortOrder = 2,
                Status = Status.Active  
            }
            );
            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation()
                {
                    Id =1,
                    CategoryId = 1,
                    Name = "Áo Nam",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-nam",
                    SeoDescription = "Sản Phẩm Aó Thời Trang Nam",
                    SeoTitle = "Sản Phẩm Aó Thời Trang Nam"
                },
                new CategoryTranslation()
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Man Shirt",
                    LanguageId = "en-US",
                    SeoAlias = "man-shirt",
                    SeoDescription = "The shirt products for man",
                    SeoTitle = "The shirt products for man"
                },
                new CategoryTranslation()
                {
                    Id = 3,
                    CategoryId = 2,
                    Name = "Áo Nữ",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-nu",
                    SeoDescription = "Sản Phẩm Aó Thời Trang Nữ",
                    SeoTitle = "Sản Phẩm Aó Thời Trang Nữ"
                },
                new CategoryTranslation()
                {
                    Id = 4,
                    CategoryId = 2,
                    Name = "Woman Shirt",
                    LanguageId = "en-US",
                    SeoAlias = "woman-shirt",
                    SeoDescription = "The shirt products for woman",
                    SeoTitle = "The shirt products for woman"
                }
                );


            modelBuilder.Entity<Product>().HasData(
            new Product ()
            {
                Id = 1,
                DateCreate = DateTime.Now,
                OriginalPrice = 100000,
                Price = 200000,
                Stock = 0,
                ViewCount = 0,
            }
            );
            modelBuilder.Entity<ProductTranslation>().HasData(
                
                    new ProductTranslation() { 
                        Id = 1,
                        ProductId = 1,
                        Name="Áo sơ mi trắng Việt Tiến",
                        LanguageId = "vi-VN",
                        SeoAlias="ao-so-mi-nam-viet_tien",
                        SeoDescription="áo trắng nam việt tiến",
                        SeoTitle="Áo sơ mi trắng việt tiến nam",
                        Details ="Áo sơ mi trắng việt tiến nam",
                        Description = ""
                    },
                    new ProductTranslation() {
                        Id = 2,
                        ProductId = 1,
                        Name ="Men Shirt Viet Tien",
                        LanguageId = "en-US",
                        SeoAlias="men-shirt-viet-tien",
                        SeoDescription="The shirt products viet tien for men",
                        SeoTitle="The shirt viet tien products for men",
                        Details ="fucked ao so mi viet tien",
                        Description = ""
                    }

                );
                modelBuilder.Entity<ProductInCategory>().HasData(
                    new  ProductInCategory() { ProductId =1,CategoryId = 1}
                );

            var roleId = new Guid("F42202B3-0E97-431C-A703-0F164EE61D9E");
            var adminId = new Guid("F8483347-1086-458E-A0E7-5D9351503B1B");
            // any guid, but nothing is to use the same 
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = adminId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = roleId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "duykhanhdeveloper93@gmail.com",
                NormalizedEmail = "duykhanhdeveloper93@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "@1234512345"),
                SecurityStamp = string.Empty,
                FirstName ="Duy Khanh",
                LastName = "Pham",
                Dob = new DateTime(2020,3,3)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });


        }
    }
}
