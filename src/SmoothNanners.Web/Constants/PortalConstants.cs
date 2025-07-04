using Blazicons;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.Pages.Shared.Portal;

namespace SmoothNanners.Web.Constants;

internal static class PortalConstants
{
    public static readonly PortalSection MusicSection = new(
        Variant.Indigo,
        "Music",
        BootstrapIcon.MusicNoteBeamed,
        [PortalLink.CreateYouTubeLink("UCPjcGsO8o_FKPGHLQ5d1hbg")],
        ["XjIJoM1AZyQ", "aPtQZNellaI"]);

    public static readonly IReadOnlyList<PortalSection> RightColumnSections =
    [
        new(
            Variant.Pink,
            "Gaming",
            BootstrapIcon.Controller,
            [
                PortalLink.CreateYouTubeLink("UCQB1XVER5WPtxRESxHMG1Qw"),
                new PortalLink(
                    Variant.Purple,
                    BootstrapIcon.Twitch,
                    "Twitch",
                    "https://www.twitch.tv/smoothnanners"),
                new PortalLink(Variant.Indigo, BootstrapIcon.Discord, "Discord", "https://discord.gg/P7rhAhA"),
                PortalLink.CreateWebsiteLink("Game Byline", "https://gamebyline.com/")
            ]),
        new(
            Variant.Cyan,
            "Programming",
            BootstrapIcon.CodeSlash,
            [
                PortalLink.CreateYouTubeLink("UCJjowuNoLywGC7ujP74zFJg"),
                new PortalLink(Variant.Slate, BootstrapIcon.Github, "GitHub", "https://github.com/austins"),
                PortalLink.CreateWebsiteLink("Austin's Dev", "https://austinsdev.com/")
            ]),
        new(
            Variant.Blue,
            "Socials",
            BootstrapIcon.PersonCircle,
            [
                new PortalLink(
                    Variant.Fuchsia,
                    BootstrapIcon.Instagram,
                    "Instagram",
                    "https://www.instagram.com/smoothnanners"),
                new PortalLink(Variant.Sky, BootstrapIcon.Telegram, "Telegram", "https://t.me/smoothnanners"),
                new PortalLink(Variant.Black, BootstrapIcon.TwitterX, "Twitter", "https://x.com/smoothnanners"),
                new PortalLink(
                    Variant.Slate,
                    BootstrapIcon.PiggyBankFill,
                    "Tip",
                    "https://streamlabs.com/smoothnanners/tip"),
                new PortalLink(Variant.Slate, BootstrapIcon.EnvelopeFill, "Email", "mailto:austin@austinsdev.com")
            ])
    ];
}
