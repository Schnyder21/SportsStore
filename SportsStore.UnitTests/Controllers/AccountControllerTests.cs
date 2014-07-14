using Moq;
using NUnit.Framework;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void LogOnWithValidCredentialsLetsUsThrough()
        {
            var controller = createController();
            var model = new LogOnViewModel { UserName = "admin", Password = "secret" };

            var result = controller.LogOn(model, "/MyURL");

            Assert.That(result, Is.TypeOf<RedirectResult>());
            Assert.That((result as RedirectResult).Url, Is.EqualTo("/MyURL"));
        }

        public void LogOnWithInvalidCredentialsAborts()
        {
            var controller = createController();
            var model = new LogOnViewModel { UserName = "admin", Password = "xxxxxx" };

            var result = controller.LogOn(model, "/MyURL");

            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That((result as ViewResult).ViewData.ModelState.IsValid, Is.True);
        }

        private static AccountController createController()
        {
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            var controller = new AccountController(mock.Object);
            return controller;
        }
    }
}
