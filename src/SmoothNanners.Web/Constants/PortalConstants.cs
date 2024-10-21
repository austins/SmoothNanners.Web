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
        BorderVariant = BootstrapVariant.Secondary,
        Links =
        [
            new PortalLink
            {
                Url = "https://www.youtube.com/channel/UCPjcGsO8o_FKPGHLQ5d1hbg",
                Text = "YouTube",
                Icon = BootstrapIconGlyph.Youtube,
                BackgroundVariant = BootstrapVariant.Danger
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
            BorderVariant = BootstrapVariant.Danger,
            Links =
            [
                new PortalLink
                {
                    Url = "https://www.youtube.com/channel/UCQB1XVER5WPtxRESxHMG1Qw",
                    Text = "YouTube",
                    Icon = BootstrapIconGlyph.Youtube,
                    BackgroundVariant = BootstrapVariant.Danger
                },
                new PortalLink
                {
                    Url = "https://www.twitch.tv/smoothnanners",
                    Text = "Twitch",
                    Icon = BootstrapIconGlyph.Twitch,
                    BackgroundVariant = BootstrapVariant.Primary
                },
                new PortalLink
                {
                    Url = "https://discord.gg/P7rhAhA",
                    Text = "Discord",
                    Icon = BootstrapIconGlyph.Discord,
                    BackgroundVariant = BootstrapVariant.Dark
                },
                new PortalLink
                {
                    Url = "https://gamebyline.com/",
                    Text = "Game Byline",
                    Icon = BootstrapIconGlyph.Globe,
                    BackgroundVariant = BootstrapVariant.Dark
                }
            ]
        },
        new PortalSection
        {
            HeadingText = "Programming",
            Icon = BootstrapIconGlyph.CodeSlash,
            BorderVariant = BootstrapVariant.Light,
            Links =
            [
                new PortalLink
                {
                    Url = "https://www.youtube.com/channel/UCJjowuNoLywGC7ujP74zFJg",
                    Text = "YouTube",
                    Icon = BootstrapIconGlyph.Youtube,
                    BackgroundVariant = BootstrapVariant.Danger
                },
                new PortalLink
                {
                    Url = "https://github.com/austins",
                    Text = "GitHub",
                    Icon = BootstrapIconGlyph.Github,
                    BackgroundVariant = BootstrapVariant.Dark
                },
                new PortalLink
                {
                    Url = "https://austinsdev.com/",
                    Text = "Austin's Dev",
                    Icon = BootstrapIconGlyph.Globe,
                    BackgroundVariant = BootstrapVariant.Dark
                }
            ]
        },
        new PortalSection
        {
            HeadingText = "Social",
            Icon = BootstrapIconGlyph.PersonCircle,
            BorderVariant = BootstrapVariant.Primary,
            Links =
            [
                new PortalLink
                {
                    Url = "https://twitter.com/smoothnanners",
                    Text = "Twitter",
                    Icon = BootstrapIconGlyph.TwitterX,
                    BackgroundVariant = BootstrapVariant.Info
                },
                new PortalLink
                {
                    Url = "https://www.instagram.com/smoothnanners",
                    Text = "Instagram",
                    Icon = BootstrapIconGlyph.Instagram,
                    BackgroundVariant = BootstrapVariant.Primary
                },
                new PortalLink
                {
                    Url = "https://streamlabs.com/smoothnanners/tip",
                    Text = "Tip",
                    Icon = BootstrapIconGlyph.CashCoin,
                    BackgroundVariant = BootstrapVariant.Dark
                },
                new PortalLink
                {
                    Url = "mailto:austin@austinsdev.com",
                    Text = "Email",
                    Icon = BootstrapIconGlyph.At,
                    BackgroundVariant = BootstrapVariant.Dark
                }
            ]
        }
    ];
}
