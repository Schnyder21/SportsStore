using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 4;

        private IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult List(string category, int page = 1)
        {
            var productsQuery = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID);
                    
            var model = new ProductsListViewModel() {
                Products = productsQuery.Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = productsQuery.Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int productID)
        {
            var prod = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            if (prod != null)
                return File(prod.ImageData, prod.ImageMimeType);

            return null;
        }
    }
}