using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace SmoothNanners.Web.Pages.Error;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public sealed class IndexModel : PageModel
{
    [FromQuery]
    [BindRequired]
    [Range(400, 599)]
    public int Code { get; init; }

    public string Message { get; private set; } = null!;

    public string RequestId { get; private set; } = null!;

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

        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        Response.StatusCode = Code;

        return Page();
    }
}
