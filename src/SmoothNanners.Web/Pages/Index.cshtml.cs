using Blazicons;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.Models.Portal;

namespace SmoothNanners.Web.Pages;

public sealed class Index : PageModel
{
    public IList<PortalCard> LeftCards { get; } =
    [
        new()
        {
            Variant = Variant.Indigo,
            HeadingIcon = BootstrapIcon.MusicNoteBeamed,
            HeadingText = "Music",
            Links = [PortalLink.YouTubeLink("UCPjcGsO8o_FKPGHLQ5d1hbg")],
            YouTubeVideoIds = ["XjIJoM1AZyQ", "aPtQZNellaI"]
        }
    ];

    public IList<PortalCard> RightCards { get; } =
    [
        new()
        {
            Variant = Variant.Pink,
            HeadingIcon = BootstrapIcon.Controller,
            HeadingText = "Gaming",
            Links =
            [
                PortalLink.YouTubeLink("UCQB1XVER5WPtxRESxHMG1Qw"),
                new PortalLink
                {
                    Variant = Variant.Purple,
                    Icon = BootstrapIcon.Twitch,
                    Text = "Twitch",
                    Url = new Uri("https://www.twitch.tv/smoothnanners")
                },
                new PortalLink
                {
                    Variant = Variant.Indigo,
                    Icon = BootstrapIcon.Discord,
                    Text = "Discord",
                    Url = new Uri("https://discord.gg/P7rhAhA")
                },
                PortalLink.WebsiteLink("Game Byline", new Uri("https://gamebyline.com/"))
            ]
        },
        new()
        {
            Variant = Variant.Cyan,
            HeadingIcon = BootstrapIcon.CodeSlash,
            HeadingText = "Programming",
            Links =
            [
                PortalLink.YouTubeLink("UCJjowuNoLywGC7ujP74zFJg"),
                new PortalLink
                {
                    Variant = Variant.Slate,
                    Icon = BootstrapIcon.Github,
                    Text = "GitHub",
                    Url = new Uri("https://github.com/austins")
                },
                PortalLink.WebsiteLink("Austin's Dev", new Uri("https://austinsdev.com/"))
            ]
        },
        new()
        {
            Variant = Variant.Blue,
            HeadingIcon = BootstrapIcon.PersonCircle,
            HeadingText = "Socials",
            Links =
            [
                new PortalLink
                {
                    Variant = Variant.Fuchsia,
                    Icon = BootstrapIcon.Instagram,
                    Text = "Instagram",
                    Url = new Uri("https://www.instagram.com/smoothnanners")
                },
                new PortalLink
                {
                    Variant = Variant.Sky,
                    Icon = BootstrapIcon.Telegram,
                    Text = "Telegram",
                    Url = new Uri("https://t.me/smoothnanners")
                },
                new PortalLink
                {
                    Variant = Variant.Black,
                    Icon = BootstrapIcon.TwitterX,
                    Text = "Twitter",
                    Url = new Uri("https://x.com/smoothnanners")
                },
                new PortalLink
                {
                    Variant = Variant.Slate,
                    Icon = BootstrapIcon.PiggyBankFill,
                    Text = "Tip",
                    Url = new Uri("https://streamlabs.com/smoothnanners/tip")
                },
                new PortalLink
                {
                    Variant = Variant.Slate,
                    Icon = BootstrapIcon.EnvelopeFill,
                    Text = "Email",
                    Url = new Uri("mailto:austin@austinsdev.com")
                }
            ]
        }
    ];

    public IActionResult OnGet()
    {
        return Page();
    }
}
