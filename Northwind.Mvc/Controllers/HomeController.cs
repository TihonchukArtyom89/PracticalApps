using Microsoft.AspNetCore.Mvc;//Controller,IActionResult
using Northwind.Mvc.Models;//ErrorViewModel
using System.Diagnostics;//Activity
using Packt.Shared;//NorthwindContext
using Microsoft.AspNetCore.Authorization;
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
        [ResponseCache(Duration = 10/*seconds*/, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            HomeIndexViewModel model = new(VisitorCount: Random.Shared.Next(1, 1001), Categories: db.Categories.ToList(), Products: db.Products.ToList());
            return View(model);//pass model to view
        }
        [Route("private")]
        [Authorize(Roles = "Administrators")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ProductDetail(int? id, string alertstyle = "success")
        {
            ViewData["alertstyle"] = alertstyle;
            if (!id.HasValue)
            {
                return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }
            Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);
            if (model is null)
            {
                return NotFound($"ProductId {id} not found.");
            }
            return View(model);//pass model a view and then return result
        }
        public IActionResult ModelBinding()
        {
            return View();//the page with form to submit
        }
        [HttpPost]//use this action method to process POSTs
        public IActionResult ModelBinding(Thing thing)
        {
            HomeModelBindingViewModel model = new(Thing: thing, HasErrors: !ModelState.IsValid, ValidationErrors: ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));
            return View(model);//show the model bound thing 
        }

    }
}