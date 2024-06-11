using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmoothNanners.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
/* [IgnoreAntiforgeryToken] */
public sealed class ErrorModel : PageModel
{
    public int Code { get; private set; } = StatusCodes.Status500InternalServerError;

    public string Message { get; private set; } = null!;

    /* public string RequestId { get; private set; } = null!; */

    public void OnGet(int? code = null)
    {
        code ??= HttpContext.Response.StatusCode;
        if (code is >= 400 and <= 599)
        {
            Code = code.Value;
        }

        Message = Code switch
        {
            StatusCodes.Status404NotFound => "The resource you are looking for was not found.",
            _ => "An error occurred while processing your request. Please try again later."
        };

        /* RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier; */
    }
}
