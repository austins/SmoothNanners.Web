namespace SmoothNanners.Web.Models.Home;

public sealed class HomeViewModel
{
    public IReadOnlyList<PortalSection> LeftColumnSections { get; init; } = [];

    public IReadOnlyList<PortalSection> RightColumnSections { get; init; } = [];
}
