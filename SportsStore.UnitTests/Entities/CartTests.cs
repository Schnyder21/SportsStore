using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using NUnit.Framework;
using SportsStore.UnitTests.TestHelpers;

namespace SportsStore.UnitTests.Entities
{
    [TestFixture]
    public class CartTests
    {
        private Product[] generateProducts(int count = 2)
        {
            return ProductHelper.GenerateProducts(count).ToArray();
        }

        [Test]
        public void CanAddNewLines()
        {
            var products = generateProducts(2);
            var p1 = products[0];
            var p2 = products[1];

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            var lines = cart.Lines.ToArray();

            Assert.That(lines.Select(c => c.Product).ToArray(), Is.EqualTo(new Product[] { p1, p2 }));
        }

        [Test]
        public void AddingMoreOfTheSameItemUpdatesTheQuantity()
        {
            var products = generateProducts(2);
            var p1 = products[0];
            var p2 = products[1];

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 10);

            var results = cart.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            Assert.That(results.Select(l => l.Quantity).ToArray(), Is.EqualTo(new int[] { 11, 1 }));
        }

        [Test]
        public void RemoveLinesTakesItOutOfTheCart()
        {
            var products = generateProducts(3);
            var p1 = products[0];
            var p2 = products[1];
            var p3 = products[2];

            var cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);

            cart.RemoveLine(p2);

            Assert.That(cart.Lines.Where(c => c.Product == p2).Count(), Is.EqualTo(0));
            Assert.That(cart.Lines.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CalculateCartTotal()
        {
            var products = generateProducts(2);
            var p1 = products[0]; p1.Price = 100M;
            var p2 = products[1]; p2.Price = 50M;
            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            Assert.That(cart.ComputeTotalValue(), Is.EqualTo(450M));
        }

        [Test]
        public void ClearEmptiesTheCart()
        {
            var cart = new Cart();
            foreach(var p in generateProducts(2))
            {
                cart.AddItem(p, 1);
            }

            cart.Clear();

            Assert.That(cart.Lines.Count(), Is.EqualTo(0));
        }
    }
}
