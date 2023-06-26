using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;//    PageModel
using Packt.Shared;//Customer, NorthwindContext
using System.Diagnostics.Metrics;

namespace PacktFeatures.Pages;

public class CustomersModel : PageModel
{
    private NorthwindContext db;
    public CustomersModel(NorthwindContext injectedContext)
    {
        db = injectedContext;
    }

    public IGrouping<string, Customer>[] Customers { get; set; } = null!;
    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Customers";
        ViewData["YearNumber"] = DateTime.Now.ToString("yyyy");
        Customers = db.Customers.GroupBy(x => x.Country).ToArray()!;
    }
}