using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;

namespace SmoothNanners.Web.Pages;

[OutputCache]
public sealed class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
