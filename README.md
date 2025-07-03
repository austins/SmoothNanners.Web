The [SmoothNanners](https://smoothnanners.com) website. A portal with links to my other websites and content.

Built with ASP.NET Core Razor Pages. The website currently has two pages (home page + error page), but may be modified to
support more content in the future. This also serves as a template for future projects.

# Testing

To run the Integration tests, run this command from the root of the solution to install the default browser:

```
pwsh ./tests/SmoothNanners.Web.Tests.Integration/bin/Debug/net9.0/playwright.ps1 install chromium
```
