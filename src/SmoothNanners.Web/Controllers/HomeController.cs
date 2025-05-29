using BootstrapIcons.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.Models.Home;

namespace SmoothNanners.Web.Controllers;

[Route("")]
public sealed class HomeController : Controller
{
    [HttpGet("")]
    [OutputCache(Duration = int.MaxValue)]
    public IActionResult Index()
    {
        return View(
            new HomeViewModel
            {
                LeftColumnSections =
                [
                    new PortalSection
                    {
                        Variant = Variant.Indigo,
                        HeadingText = "Music",
                        HeadingIcon = BootstrapIconGlyph.MusicNoteBeamed,
                        PortalLinks = [PortalLink.CreateYouTubeLink("UCPjcGsO8o_FKPGHLQ5d1hbg")],
                        YouTubeVideoIds = ["XjIJoM1AZyQ", "aPtQZNellaI"]
                    }
                ],
                RightColumnSections =
                [
                    new PortalSection
                    {
                        Variant = Variant.Pink,
                        HeadingText = "Gaming",
                        HeadingIcon = BootstrapIconGlyph.Controller,
                        PortalLinks =
                        [
                            PortalLink.CreateYouTubeLink("UCQB1XVER5WPtxRESxHMG1Qw"),
                            new PortalLink
                            {
                                Variant = Variant.Purple,
                                Icon = BootstrapIconGlyph.Twitch,
                                Url = new Uri("https://www.twitch.tv/smoothnanners"),
                                Text = "Twitch"
                            },
                            new PortalLink
                            {
                                Variant = Variant.Indigo,
                                Icon = BootstrapIconGlyph.Discord,
                                Url = new Uri("https://discord.gg/P7rhAhA"),
                                Text = "Discord"
                            },
                            PortalLink.CreateWebsiteLink(new Uri("https://gamebyline.com/"), "Game Byline")
                        ]
                    },
                    new PortalSection
                    {
                        Variant = Variant.Cyan,
                        HeadingText = "Programming",
                        HeadingIcon = BootstrapIconGlyph.CodeSlash,
                        PortalLinks =
                        [
                            PortalLink.CreateYouTubeLink("UCJjowuNoLywGC7ujP74zFJg"),
                            new PortalLink
                            {
                                Variant = Variant.Slate,
                                Icon = BootstrapIconGlyph.Github,
                                Url = new Uri("https://github.com/austins"),
                                Text = "GitHub"
                            },
                            PortalLink.CreateWebsiteLink(new Uri("https://austinsdev.com/"), "Austin's Dev")
                        ]
                    },
                    new PortalSection
                    {
                        Variant = Variant.Blue,
                        HeadingText = "Socials",
                        HeadingIcon = BootstrapIconGlyph.PersonCircle,
                        PortalLinks =
                        [
                            new PortalLink
                            {
                                Variant = Variant.Fuchsia,
                                Icon = BootstrapIconGlyph.Instagram,
                                Url = new Uri("https://www.instagram.com/smoothnanners"),
                                Text = "Instagram"
                            },
                            new PortalLink
                            {
                                Variant = Variant.Sky,
                                Icon = BootstrapIconGlyph.Telegram,
                                Url = new Uri("https://t.me/smoothnanners"),
                                Text = "Telegram"
                            },
                            new PortalLink
                            {
                                Variant = Variant.Black,
                                Icon = BootstrapIconGlyph.TwitterX,
                                Url = new Uri("https://twitter.com/smoothnanners"),
                                Text = "Twitter"
                            },
                            new PortalLink
                            {
                                Variant = Variant.Slate,
                                Icon = BootstrapIconGlyph.PiggyBankFill,
                                Url = new Uri("https://streamlabs.com/smoothnanners/tip"),
                                Text = "Tip"
                            },
                            new PortalLink
                            {
                                Variant = Variant.Slate,
                                Icon = BootstrapIconGlyph.EnvelopeFill,
                                Url = new Uri("mailto:austin@austinsdev.com"),
                                Text = "Email"
                            }
                        ]
                    }
                ]
            });
    }
}
