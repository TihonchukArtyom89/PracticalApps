using Microsoft.AspNetCore.Mvc;//[ApiController],[Route]
using Microsoft.EntityFrameworkCore;//ToListAsync,FirstOrDefaultAsync
using Packt.Shared;//NorthwindContext,Customer

namespace Northwind.BlazorWasm.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CustomersController:ControllerBase
{
    private readonly NorthwindContext db;
    public CustomersController(NorthwindContext db)
    {
        this.db = db;
    }
    [HttpGet]
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await db.Customers.ToListAsync();
    }
    [HttpGet("in/{country}")]//different path to disambiguate
    public async Task<List<Customer>> GetCustomersAsync(string country)
    {
        return await db.Customers.Where(c=>c.Country==country).ToListAsync();
    }
}
