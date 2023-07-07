﻿using Microsoft.AspNetCore.Mvc;//Controller,IActionResult
using Northwind.Mvc.Models;//ErrorViewModel
using System.Diagnostics;//Activity
using Packt.Shared;//NorthwindContext
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;//Include extension method
using System.Linq;
using System.Text.RegularExpressions; // Regex
using Microsoft.Extensions.Logging;

namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext db;
        private readonly IHttpClientFactory clientFactory;
        public HomeController(ILogger<HomeController> logger, NorthwindContext injectedContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = injectedContext;
            clientFactory = httpClientFactory;
        }
        [ResponseCache(Duration = 10/*seconds*/, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            _logger.LogError("This is a serious error (not really!)");
            _logger.LogWarning("This is your first warning!");
            _logger.LogWarning("Second warning!");
            _logger.LogInformation("I am in the Index method of the HomeController.");
            HomeIndexViewModel model = new(VisitorCount: Random.Shared.Next(1, 1001), Categories: await db.Categories.ToListAsync(), Products: await db.Products.ToListAsync());
            try
            {
                HttpClient client = clientFactory.CreateClient(name: "Minimal.WebApi");
                HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: "weatherforecast");
                HttpResponseMessage response = await client.SendAsync(request);
                ViewData["weather"] = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"The Minimal Web.Api is not responding. Exception: {ex.Message}");
                ViewData["weather"] = Enumerable.Empty<WeatherForecast>().ToArray();
            }
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
        public async Task<IActionResult> ProductDetail(int? id, string alertstyle = "success")
        {
            ViewData["alertstyle"] = alertstyle;
            if (!id.HasValue)
            {
                return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }
            Product? model = await db.Products.SingleOrDefaultAsync(p => p.ProductId == id);
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
        public IActionResult ProductsThatCostMoreThan(decimal? price)
        {
            if (!price.HasValue)
            {
                return BadRequest("You must pass a product price in the query string, for example /Home/ProductsThatCostMoreThan?price=50");
            }
            IEnumerable<Product> model = db.Products.Include(p => p.Category).Include(p => p.Supplier).Where(p => p.UnitPrice > price);
            if (!model.Any())
            {
                return NotFound($"No products cost more than {price:C}");
            }
            ViewData["MaxPrice"] = price.Value.ToString("C");
            return View(model);//Pass model to view
        }
        [Route("category/{id}")]
        [Route("category")]
        public async Task<IActionResult> Category(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must pass a category ID in the route, for example, /Home/Category/2");
            }
            Category? model = await db.Categories.Include(p => p.Products).SingleOrDefaultAsync(p => p.CategoryId == id);
            if (model is null)
            {
                return NotFound($"CategoryId {id} not found.");
            }
            return View(model);//pass model a view and then return result
        }
        public async Task<IActionResult> Customers(string country)
        {
            string uri;
            if (string.IsNullOrEmpty(country))
            {
                ViewData["Title"] = "All Customers Worldwide";
                uri = "api/customers";
            }
            else
            {
                ViewData["Title"] = $"Customers in {country}";
                uri = $"api/customers/?country={country}";
            }
            HttpClient client = clientFactory.CreateClient(name: "Northwind.WebApi");
            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: uri);
            HttpResponseMessage response = await client.SendAsync(request);
            IEnumerable<Customer>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
            return View(model);
        }
        public async Task<IActionResult> CreateCustomer(Customer c)
        {
            HttpClient client = clientFactory.CreateClient(name: "Northwind.WebApi");
            HttpRequestMessage request = new(method: HttpMethod.Post, requestUri: "api/customers");
            request.Content = JsonContent.Create(c);
            HttpResponseMessage response = await client.SendAsync(request);
            Customer? customer = await response.Content.ReadFromJsonAsync<Customer>();
            return View(customer);
        }
        public async Task<IActionResult> SearchCustomer(string searchPhrase)
        {
            HttpClient client = clientFactory.CreateClient(name: "Northwind.WebApi");
            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: "api/customers");
            HttpResponseMessage response = await client.SendAsync(request);
            List<Customer>? allCustomers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            List<Customer>? resultCustomers = new();
            if (allCustomers is not null)
            {
                Regex regex = new Regex(searchPhrase,RegexOptions.IgnoreCase);
                foreach (Customer c in allCustomers)
                {//search will be on CustomerId and CompanyName
                    MatchCollection matchesCustomerId = regex.Matches(c.CustomerId);//Regex.Match(c.CustomerId, $"{searchPhrase}");
                    MatchCollection matchesCompanyName = regex.Matches(c.CompanyName);
                    if ((matchesCustomerId.Count!=0)||(matchesCompanyName.Count!=0))
                    {//add this customer to search result list (List<Customer>? resultCustomers)
                        resultCustomers.Add(c);
                    }
                }
            }
            //customers = customers.Where()
            return View(resultCustomers);
        }
    }
}