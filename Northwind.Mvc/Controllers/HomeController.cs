using Microsoft.AspNetCore.Mvc;//Controller,IActionResult
using Northwind.Mvc.Models;//ErrorViewModel
using System.Diagnostics;//Activity
using Packt.Shared;//NorthwindContext
namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext db;  

        public HomeController(ILogger<HomeController> logger, NorthwindContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel model = new(VisitorCount:Random.Shared.Next(1,1001),Categories:db.Categories.ToList(),Products:db.Products.ToList());
            return View(model);//pass model to view
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ProductDetail(int? id)
        {
            if(!id.HasValue)
            {
                return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }
            Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);
            if(model is null)
            {
                return NotFound($"ProductId {id} not found.");
            }
            return View(model);//pass model a view and then return result
        }
    }
}