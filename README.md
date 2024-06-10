The [SmoothNanners](https://smoothnanners.com) website. A portal with links to my other websites and content.

Built with ASP.NET Core Razor Pages with TailwindCSS and Alpine.js. JavaScript dependency management is handled via PNPM
and bundled with Vite. The app is statically generated and deployed to GitHub Pages. The website is currently single
page (+page for 404) and can be modified to run as a server instance with more content in the future. This also serves
as a template for future Razor Pages projects.

# Testing

To run the E2E tests, run this command from the root of the solution to install the default browser:

```
pwsh ./SmoothNanners.Web.Tests.E2E/bin/Debug/net8.0/playwright.ps1 install chromium
```
