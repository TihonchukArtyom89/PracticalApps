using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Web.Pages
{
    public class PrimeFactorsModel : PageModel
    {
        [BindProperty]
        public int? Number { get; set; } = null;
        public string? Answer { get; set; } = null;
        public void OnGet()
        {
            ViewData["YearNumber"] = DateTime.Now.ToString("yyyy");
            ViewData["Title"] = "Northwind B2B - Prime Factors";
        }
        public void OnPost()
        {
            if(Number is not null)
            {
                Answer=Calculate.PrintPrimeFactors(Number);
                ViewData["YearNumber"] = DateTime.Now.ToString("yyyy");
                ViewData["Title"] = "Northwind B2B - Prime Factors";
            }
        }
    }
}
