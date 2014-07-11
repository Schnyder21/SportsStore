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
            return new CartController(mock.Object, null);
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

        [Test]
        public void CannotCheckoutEmptyCart()
        {
            var mock = new Mock<IOrderProcessor>();
            var cart = new Cart();
            var shippingDetails = new ShippingDetails();
            var controller = new CartController(null, mock.Object);

            var result = controller.Checkout(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.That(result.ViewName, Is.EqualTo(""));
            Assert.That(result.ViewData.ModelState.IsValid, Is.False);
        }

        [Test]
        public void CannotCheckoutWithInvalidShippingDetails()
        {
            var mock = new Mock<IOrderProcessor>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            var controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            var result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.That(result.ViewName, Is.Empty);
            Assert.That(result.ViewData.ModelState.IsValid, Is.False);
        }

        [Test]
        public void SubmittingAValidOrderInvokesTheOrderProcessor()
        {
            var mock = new Mock<IOrderProcessor>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            var controller = new CartController(null, mock.Object);

            var result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            Assert.That(result.ViewName, Is.EqualTo("Completed"));
            Assert.That(result.ViewData.ModelState.IsValid, Is.True);
        }
    }
}
