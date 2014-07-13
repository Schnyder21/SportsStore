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

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        [Test]
        public void IndexContainsAllProducts()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.Products).Returns(ProductHelper.GenerateProducts(3));
            var controller = new AdminController(mock.Object);

            var result = (controller.Index().ViewData.Model as IEnumerable<Product>).ToArray();

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("P1"));
            Assert.That(result[1].Name, Is.EqualTo("P2"));
            Assert.That(result[2].Name, Is.EqualTo("P3"));
        }
    }
}
