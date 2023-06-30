using Microsoft.AspNetCore.Mvc;//[Route],[ApiController],ControllerBase
using Packt.Shared;//Customer
using Northwind.WebApi.Repositories;//ICustomerRepository
namespace Northwind.WebApi.Controllers;

//base address: api/customers
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository repo;
    //constructor injects repository registered in Startup
    public CustomersController(ICustomerRepository repo)
    {
        this.repo = repo;
    }
    //GET: api/customers
    //GET: api/customers/?country=[country]
    //this will always return a list of customers (but it might be empty)
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
    public async Task<IEnumerable<Customer>> GetCustomers(string? country)
    {
        if (string.IsNullOrEmpty(country))
        {
            return await repo.RetrieveAllAsync();
        }
        else
        {

            return (await repo.RetrieveAllAsync()).Where(customer => customer.Country == country);
        }
    }
    //GET: api/customers/[id]
    [HttpGet("{id}", Name = nameof(GetCustomer))]//named route
    [ProducesResponseType(200, Type = typeof(Customer))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomer(string id)
    {
        Customer? c = await repo.RetrieveAsync(id);
        if (c == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(c);
        }
    }
    //POST: api/customers 
    //BODY: Customer(JSON,XML)
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Customer))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Create([FromBody] Customer c)
    {
        if (c == null)
        {
            return BadRequest();//400 Bad request
        }
        Customer? addedCustomer = await repo.CreateAsync(c);
        if (addedCustomer == null)
        {
            return BadRequest("Repository failed to create customer.");//400 Bad request
        }
        else
        {
            return CreatedAtRoute(//201 Created 
                routeName: nameof(GetCustomer),
                routeValues: new { id = addedCustomer.CustomerId.ToLower() }, value: addedCustomer);
        }
    }
    //PUT: api/customers 
    //BODY: Customer(JSON,XML)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(string id, [FromBody] Customer c)
    {
        id = id.ToUpper();
        c.CustomerId = c.CustomerId.ToUpper();
        if(c==null || c.CustomerId!=id)
        {
            return BadRequest();//400 Bad request
        }
        Customer? existing = await repo.RetrieveAsync(id);
        if(existing == null) 
        {
            return NotFound();
        }
        await  repo.UpdateAsync(id, c);
        return new NoContentResult();//204 No Content
    }
    //DELETE: api/customers/[id]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
        //take control  of problem details 
        if(id=="bad")
        {
            ProblemDetails problemDetails = new() 
            {
                Status=StatusCodes.Status400BadRequest,
                Type="Https://localhost:5001/customers/failed-to-delete",
                Title=$"Customer { id } found but failed to delete",
                Detail= "More details like Company Name,Country and so on.",
                Instance=HttpContext.Request.Path
            };
            return BadRequest(problemDetails);//400 Bad request
        }
        Customer? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound();//Resource not found
        }
        bool? deleted = await repo.DeleteAsync(id);
        if(deleted.HasValue && deleted.Value)//short circuit AND
        {
            return new NoContentResult();//204 No Content
        }
        else
        {
            return BadRequest($"Customer {id} was found but failed to delete");//400 Bad request
        }
    }
}
