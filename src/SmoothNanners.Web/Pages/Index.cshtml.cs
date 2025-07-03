using Microsoft.AspNetCore.OutputCaching;

namespace SmoothNanners.Web.Pages;

[OutputCache(Duration = int.MaxValue)]
public sealed class Index : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
