using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.UnitTests.TestHelpers;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class CartControllerTests
    {
        private CartController createCartController()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(ProductHelper.GenerateProducts(1));
            return new CartController(mock.Object);
        }

        [Test]
        public void AddToCartActionUpdatesCart()
        {
            var controller = createCartController();
            var cart = new Cart();

            controller.AddToCart(cart, 1, null);

            Assert.That(cart.Lines.Count(), Is.EqualTo(1));
            Assert.That(cart.Lines.First().Product.ProductID, Is.EqualTo(1));
        }

        [Test]
        public void AddProductToCartGoesToCartScreen()
        {
            var controller = createCartController();
            var cart = new Cart();

            var result = controller.AddToCart(cart, 1, "myUrl");

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["returnUrl"], Is.EqualTo("myUrl"));
        }

        [Test]
        public void IndexViewsCartContents()
        {
            var controller = createCartController();
            var cart = new Cart();

            var result = (CartIndexViewModel)controller.Index(cart, "myUrl").ViewData.Model;

            Assert.That(result.Cart, Is.EqualTo(cart));
            Assert.That(result.ReturnUrl, Is.EqualTo("myUrl"));
        }
    }
}
