using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.UnitTests.TestHelpers;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        private IProductRepository mockRepostory(int count)
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.Products).Returns(ProductHelper.GenerateProducts(3));
            return mock.Object;
        }
        [Test]
        public void IndexContainsAllProducts()
        {
            var controller = new AdminController(mockRepostory(3));

            var result = (controller.Index().ViewData.Model as IEnumerable<Product>).ToArray();

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("P1"));
            Assert.That(result[1].Name, Is.EqualTo("P2"));
            Assert.That(result[2].Name, Is.EqualTo("P3"));
        }

        [Test]
        public void EditActionSetsUpTheProductByID()
        {
            var controller = new AdminController(mockRepostory(3));

            var p1 = controller.Edit(1).ViewData.Model as Product;
            var p2 = controller.Edit(2).ViewData.Model as Product;
            var p3 = controller.Edit(3).ViewData.Model as Product;

            Assert.That(p1.ProductID, Is.EqualTo(1));
            Assert.That(p2.ProductID, Is.EqualTo(2));
            Assert.That(p3.ProductID, Is.EqualTo(3));

        }

        [Test]
        public void EditActionOnNonExistantProductReturnsNullView()
        {
            var controller = new AdminController(mockRepostory(3));

            var result = controller.Edit(4).ViewData.Model as Product;

            Assert.That(result, Is.Null);
        }

        [Test]
        public void EditActionWithValidChangesAreSaved()
        {
            var mock = new Mock<IProductRepository>();
            var controller = new AdminController(mock.Object);
            var product = new Product { Name = "Test" };

            var result = controller.Edit(product, null);

            mock.Verify(m => m.SaveProduct(product));
            Assert.That(result, Is.Not.TypeOf<ViewResult>());
        }

        [Test]
        public void EditActionWithInvalidChangesAreNotSaved()
        {
            var mock = new Mock<IProductRepository>();
            var controller = new AdminController(mock.Object);
            var product = new Product { Name = "Test" };
            controller.ModelState.AddModelError("error", "error");

            var result = controller.Edit(product, null);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void DeleteActionWithValidProductCallsTheRepositoryDeleteMethod()
        {
            //Need to get a Queryable view over the same in memory list of Products
            var products = ProductHelper.GenerateProducts(3).ToArray().AsQueryable();
            var product = products.First();
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products);
            var controller = new AdminController(mock.Object);

            Assert.That(product, Is.EqualTo(mock.Object.Products.FirstOrDefault(p => p.ProductID == product.ProductID)));

            controller.Delete(product.ProductID);

            mock.Verify(m => m.DeleteProduct(product));
        }

        [Test]
        public void DeleteActionWithInvalidProductDoesNothing()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(ProductHelper.GenerateProducts(3));
            var controller = new AdminController(mock.Object);

            controller.Delete(100);

            mock.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never());
        }
    }
}
