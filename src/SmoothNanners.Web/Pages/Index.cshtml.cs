using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmoothNanners.Web.Pages;

public sealed class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
