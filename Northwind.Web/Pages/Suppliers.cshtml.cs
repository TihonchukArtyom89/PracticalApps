using Microsoft.AspNetCore.Mvc;//[BindProperty], IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages;//PageModel
using Packt.Shared;

namespace Northwind.Web.Pages
{
    public class SuppliersModel : PageModel
    {
        private NorthwindContext db;
        public SuppliersModel(NorthwindContext injectedContext)
        {
            db = injectedContext;
        }
        public IEnumerable<Supplier>? Suppliers { get; set; }
        public void OnGet()
        {
            ViewData["YearNumber"] = DateTime.Now.ToString("yyyy");
            ViewData["Title"] = "Northwind B2B - Suppliers";
            Suppliers = db.Suppliers.OrderBy(c => c.Country).ThenBy(c => c.CompanyName);//new[] { "Alpha Co", "Beta Limited", "Gamma Corp" };
        }
        [BindProperty]
        public Supplier? Supplier { get; set; }
        public IActionResult OnPost()
        {
            if ((Supplier is not null) && ModelState.IsValid)
            {
                db.Suppliers.Add(Supplier);
                db.SaveChanges();
                return RedirectToPage("/suppliers");
            }
            else
            {
                return Page();//return to original page
            }
        }
    }
}
