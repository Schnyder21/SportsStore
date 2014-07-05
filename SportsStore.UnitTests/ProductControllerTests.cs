using System;
using NUnit.Framework;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;

namespace SportsStore.UnitTests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private IQueryable<Product> products()
        {
            return Enumerable.Range(1, 5)
                .Select(i => new Product { ProductID = i, Name = "P" + i })
                .AsQueryable();
        }
        [Test]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = (IEnumerable<Product>)controller.List(2).Model;

            var prodArray = result.ToArray();
            Assert.That(prodArray.Length, Is.EqualTo(2));
            Assert.That(prodArray[0].Name, Is.EqualTo("P4"));
            Assert.That(prodArray[1].Name, Is.EqualTo("P5"));
        }
    }
}
