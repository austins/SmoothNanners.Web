using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmoothNanners.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public sealed class ErrorModel : PageModel
{
    [FromQuery]
    [BindRequired]
    [Range(400, 599)]
    public int Code { get; set; }

    public string Message { get; private set; } = null!;

    public IActionResult OnGet()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Message = Code switch
        {
            StatusCodes.Status404NotFound => "The resource you are looking for was not found.",
            _ => "An error occurred while processing your request. Please try again later."
        };

        // Force return 200 for 404 page SSG generation, which requires a success response.
        Response.StatusCode = Code is StatusCodes.Status404NotFound ? StatusCodes.Status200OK : Code;

        return Page();
    }
}
