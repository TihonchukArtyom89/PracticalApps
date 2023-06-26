using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.Shared;

namespace PacktFeatures.Pages;

public class CustomerDetailsModel : PageModel
{
    private NorthwindContext db;
    public CustomerDetailsModel(NorthwindContext injectedContext)
    {
        db = injectedContext;
    }
    public Customer? Customer { get; set; } = null;
    public List<Order>? Orders { get; set; } = null;
    public void OnGet(string customerId)
    {
        ViewData["YearNumber"] = DateTime.Now.ToString("yyyy");
        Customer = db.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
        ViewData["Title"] = $"Northwind B2B - Customer Details: {Customer?.CompanyName}";
        Orders = db.Orders.Where(x => x.CustomerId == customerId).ToList();
    }
}

