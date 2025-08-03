using AspNetStatic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

namespace SmoothNanners.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public sealed class Error : PageModel
{
    [FromQuery]
    [BindRequired]
    [Range(400, 599)]
    public HttpStatusCode Code { get; init; }

    public string Message { get; private set; } = null!;

    public string? RequestId { get; private set; }

    public IActionResult OnGet()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Message = Code switch
        {
            HttpStatusCode.NotFound => "The page you are looking for was not found.",
            _ => "An error occurred while processing your request. Please try again later."
        };

        if (!Environment.GetCommandLineArgs().HasExitWhenDoneArg())
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            Response.StatusCode = (int)Code;
        }

        return Page();
    }
}
