using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests.HtmlHelpers
{
    [TestFixture]
    public class PagingHelpersTests
    {
        [Test]
        public void CanGeneratePageLinks()
        {
            HtmlHelper html = null;
            var pagingInfo = new PagingInfo{
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            var result = html.PageLinks(pagingInfo, i => "Page" + i);

            Assert.That(result.ToString(), Is.EqualTo(@"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>"));
        }
    }
}
