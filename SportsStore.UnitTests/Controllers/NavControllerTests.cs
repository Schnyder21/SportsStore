using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class NavControllerTests
    {
        [Test]
        public void CanCreateCategories()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 2, Name = "P2", Category = "Apples" },
                new Product { ProductID = 3, Name = "P3", Category = "Plums" },
                new Product { ProductID = 4, Name = "P4", Category = "Oranges" }
            }.AsQueryable());

            var controller = new NavController(mock.Object);

            var results = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.That(results, Is.EqualTo(new string[] { "Apples", "Oranges", "Plums" }));
        }

        public void IndicatesSelectedCategory()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 2, Name = "P2", Category = "Oranges" }
            }.AsQueryable());

            var controller = new NavController(mock.Object);

            var category = "Apples";
            string result = controller.Menu(category).ViewBag.SelectedCategory;

            Assert.That(result, Is.EqualTo(category));
        }
    }
}
