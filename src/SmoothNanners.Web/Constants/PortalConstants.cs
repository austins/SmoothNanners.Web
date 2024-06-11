using BootstrapIcons.Net;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.Models.Portal;

namespace SmoothNanners.Web.Constants;

/// <summary>
/// Constants for the home page portal.
/// </summary>
internal static class PortalConstants
{
    public static readonly PortalSection MusicSection = new()
    {
        HeadingText = "Music",
        Icon = BootstrapIconGlyph.MusicNoteBeamed,
        BorderVariant = ColorVariant.Fuchsia,
        Links =
        [
            new PortalLink
            {
                Url = "https://www.youtube.com/channel/UCPjcGsO8o_FKPGHLQ5d1hbg",
                Text = "YouTube",
                Icon = BootstrapIconGlyph.Youtube,
                BackgroundVariant = ColorVariant.Rose
            }
        ],
        YouTubeVideoIds = ["lnWtTXtXc0A", "XjIJoM1AZyQ"]
    };

    public static readonly IReadOnlyList<PortalSection> RightColumnSections =
    [
        new PortalSection
        {
            HeadingText = "Gaming",
            Icon = BootstrapIconGlyph.Controller,
            BorderVariant = ColorVariant.Purple,
            Links =
            [
                new PortalLink
                {
                    Url = "https://www.youtube.com/channel/UCQB1XVER5WPtxRESxHMG1Qw",
                    Text = "YouTube",
                    Icon = BootstrapIconGlyph.Youtube,
                    BackgroundVariant = ColorVariant.Rose
                },
                new PortalLink
                {
                    Url = "https://www.twitch.tv/smoothnanners",
                    Text = "Twitch",
                    Icon = BootstrapIconGlyph.Twitch,
                    BackgroundVariant = ColorVariant.Purple
                },
                new PortalLink
                {
                    Url = "https://discord.gg/P7rhAhA",
                    Text = "Discord",
                    Icon = BootstrapIconGlyph.Discord,
                    BackgroundVariant = ColorVariant.Indigo
                },
                new PortalLink
                {
                    Url = "https://gamebyline.com/",
                    Text = "Game Byline",
                    Icon = BootstrapIconGlyph.Globe,
                    BackgroundVariant = ColorVariant.Indigo
                }
            ]
        },
        new PortalSection
        {
            HeadingText = "Programming",
            Icon = BootstrapIconGlyph.CodeSlash,
            BorderVariant = ColorVariant.Sky,
            Links =
            [
                new PortalLink
                {
                    Url = "https://www.youtube.com/channel/UCJjowuNoLywGC7ujP74zFJg",
                    Text = "YouTube",
                    Icon = BootstrapIconGlyph.Youtube,
                    BackgroundVariant = ColorVariant.Rose
                },
                new PortalLink
                {
                    Url = "https://github.com/austins",
                    Text = "GitHub",
                    Icon = BootstrapIconGlyph.Github,
                    BackgroundVariant = ColorVariant.Indigo
                },
                new PortalLink
                {
                    Url = "https://austinsdev.com/",
                    Text = "Austin's Dev",
                    Icon = BootstrapIconGlyph.Globe,
                    BackgroundVariant = ColorVariant.Indigo
                }
            ]
        },
        new PortalSection
        {
            HeadingText = "Social",
            Icon = BootstrapIconGlyph.PersonCircle,
            BorderVariant = ColorVariant.Indigo,
            Links =
            [
                new PortalLink
                {
                    Url = "https://twitter.com/smoothnanners",
                    Text = "Twitter",
                    Icon = BootstrapIconGlyph.Twitter,
                    BackgroundVariant = ColorVariant.Sky
                },
                new PortalLink
                {
                    Url = "https://streamlabs.com/smoothnanners/tip",
                    Text = "Tip",
                    Icon = BootstrapIconGlyph.CashCoin,
                    BackgroundVariant = ColorVariant.Indigo
                },
                new PortalLink
                {
                    Url = "mailto:austin@austinsdev.com",
                    Text = "Email",
                    Icon = BootstrapIconGlyph.At,
                    BackgroundVariant = ColorVariant.Indigo
                }
            ]
        }
    ];
}
