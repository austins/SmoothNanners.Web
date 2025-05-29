using Microsoft.AspNetCore.Mvc;
using SmoothNanners.Web.Models.Error;
using System.Diagnostics;

namespace SmoothNanners.Web.Controllers;

[Route("error")]
public sealed class ErrorController : Controller
{
    [HttpGet("")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index([FromQuery] int? code = null)
    {
        if (!ModelState.IsValid || code is null or < 400 or > 599)
        {
            code = StatusCodes.Status500InternalServerError;
        }

        Response.StatusCode = code.Value;

        return View(
            new ErrorViewModel
            {
                Code = code.Value,
                Message = code switch
                {
                    StatusCodes.Status404NotFound => "The resource you are looking for was not found.",
                    _ => "An error occurred while processing your request. Please try again later."
                },
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
