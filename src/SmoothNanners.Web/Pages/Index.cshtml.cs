using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmoothNanners.Web.Pages;

public sealed class Index : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
