using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            var categories = repository.Products
                            .Select(p => p.Category)
                            .Distinct()
                            .OrderBy(c => c);
            return PartialView(categories);
        }
    }
}