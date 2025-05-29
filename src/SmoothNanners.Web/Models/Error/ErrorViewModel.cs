namespace SmoothNanners.Web.Models.Error;

public sealed class ErrorViewModel
{
    public required int Code { get; init; }

    public required string Message { get; init; }

    public required string RequestId { get; init; }
}
