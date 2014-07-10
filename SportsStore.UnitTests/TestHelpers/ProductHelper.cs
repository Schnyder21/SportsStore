using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.UnitTests.TestHelpers
{
    internal class ProductHelper
    {
        public static IQueryable<Product> GenerateProducts(int count = 2)
        {
            return Enumerable.Range(1, count).Select(i =>
                new Product
                {
                    ProductID = i,
                    Name = "P" + i,
                    Category = "Apples"
                }).AsQueryable();
        }
    }
}
