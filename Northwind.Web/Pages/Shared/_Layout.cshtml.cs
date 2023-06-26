using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Web.Pages.Shared
{
    public class _LayoutModel : PageModel
    {
        public void OnGet()
        {
            //ViewData["YearNumber"] = DateTime.Now.ToString("yyyy");
        }
    }
}
