using System;
using NUnit.Framework;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using SportsStore.WebUI.Models;
using System.Web.Mvc;
using SportsStore.UnitTests.TestHelpers;

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private IQueryable<Product> products()
        {
            return Enumerable.Range(1, 5)
                .Select(i => new Product
                { 
                    ProductID = i,
                    Name = "P" + i,
                    Category = "Cat" + (((i-1) % 3) + 1)
                }).AsQueryable();
        }

        [Test]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            var prodArray = result.Products.ToArray();
            Assert.That(prodArray.Length, Is.EqualTo(2));
            Assert.That(prodArray[0].Name, Is.EqualTo("P4"));
            Assert.That(prodArray[1].Name, Is.EqualTo("P5"));

            var pagingInfo = result.PagingInfo;
            Assert.That(pagingInfo.CurrentPage, Is.EqualTo(2));
            Assert.That(pagingInfo.ItemsPerPage, Is.EqualTo(3));
            Assert.That(pagingInfo.TotalItems, Is.EqualTo(5));
            Assert.That(pagingInfo.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public void CanFilterProducts()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(this.products());

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("P2"));
            Assert.That(result[0].Category, Is.EqualTo("Cat2"));
            Assert.That(result[1].Name, Is.EqualTo("P5"));
            Assert.That(result[1].Category, Is.EqualTo("Cat2"));
        }

        [Test]
        public void GenerateCategorySpecificProductCount()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(products());

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Assert.That(categoryCount(controller, "Cat1"), Is.EqualTo(2), "Should be 2 cat1 records");
            Assert.That(categoryCount(controller, "Cat2"), Is.EqualTo(2), "Should be 2 cat2 records");
            Assert.That(categoryCount(controller, "Cat3"), Is.EqualTo(1), "Should be 1 cat3 records");
            Assert.That(categoryCount(controller, null), Is.EqualTo(5), "should be 5 total records");
        }

        [Test]
        public void RetrieveImageDataSetsDataAndMimeType()
        {
            var prod = new Product{
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[0],
                ImageMimeType = "image/png"
            };
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID = 1, Name = "P1"},
                prod,
                new Product{ProductID = 2, Name="P2"}
            }.AsQueryable());
            var controller = new ProductController(mock.Object);

            var result = controller.GetImage(2);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<FileContentResult>());
            Assert.That((result as FileResult).ContentType, Is.EqualTo(prod.ImageMimeType));
        }
        
        [Test]
        public void DoesntRetrieveImnageDataForWrongProducts()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(ProductHelper.GenerateProducts(3));
            var controller = new ProductController(mock.Object);

            var result = controller.GetImage(1000);

            Assert.That(result, Is.Null);
        }

        private int categoryCount(ProductController controller, string category)
        {
            return ((ProductsListViewModel)controller.List(category, 1).Model).PagingInfo.TotalItems;
        }
    }
}
