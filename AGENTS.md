# AGENTS.md

Razor Pages site, statically generated with AspNetStatic and deployed as plain HTML, so every page renders on GET with no per-request state.

Done means `dotnet test -c Release` is green, as in CI.

Concrete classes are `sealed`. Warnings are errors: fix the cause, and scope a justified suppression with `#pragma warning disable/restore`.

## Static generation

Register every new page and `wwwroot` asset in the `StaticResourcesInfoProvider` list in `Program.cs`; the generated site contains only what is listed.

## Pages

- Page models are named after the page (`Index`, not `IndexModel`).
- Link with SafeRouting: `<a for-route="@Routes.Pages.X.Get()">` in Razor, `GetPath(Routes.Pages.X.Get())` in tests.
- Set titles with `ViewData.SetTitle(...)`; shared strings live in `AppConstants`.

## Components

Follow [StaticComponents](https://static-components.techgems.net/introduction/overview/): `X.cshtml` + `X.cshtml.cs` + `X.cshtml.css`. Use `StaticComponent` for a tag with child content, `StaticNode` with `[HtmlTargetElement("x", TagStructure = TagStructure.WithoutEndTag)]` for a self-closing one. HTML concerns stay in the template, not the model.

- Ids for one-off elements, classes for repeated ones.
- Icons: `<icon svg-icon="Ionicon.X"/>`; outline for headings, filled or logo for links.
- JS is progressive enhancement with Alpine: the markup works without it.

## Styling

- Colour and layout tokens live in `:root` in `wwwroot/site.css`. Sections set `--accent`; components read `var(--accent)` and tint with `color-mix(in srgb, var(--x) N%, transparent)`.
- `rem` sizes, fluid via `clamp()`; logical properties; mobile-first with one `min-width: 52rem` breakpoint.
- Scoped CSS selects plain elements and child combinators (`& > img`); the scope keeps them local.
- Native nesting: put `@media` inside the rule it changes.
- One-line comments give the reason for any non-obvious rule.

Every interactive style honours user preferences:

- `:focus-visible` matches hover; hover sits in `@media (hover: hover)`.
- Motion sits in `@media (prefers-reduced-motion: no-preference)`.
- `prefers-contrast: more` drops glows; `forced-colors: active` sets SVG icons to system colours (`LinkText`, `CanvasText`).

Scoping gotchas:

- `<html>` and `<body>` aren't scoped: style them in `site.css`.
- Tag-helper output (e.g. `<a for-route>`) has no scope attribute: reach it with `::deep`.
- Bare declarations in a nested `@media` get the scope attribute appended as if they were selectors: wrap them in `& { }`.
- The AspNetStatic minifier mangles `::deep :pseudo`: put an element or class between them.

## Dependencies

- Front-end libraries come from `libman.json`, restored into `wwwroot/vendor` on build.
- Commit `packages.lock.json` with any package change; CI restores in `--locked-mode`.

## Tests

xUnit v3 + Playwright E2E in `tests/SmoothNanners.Web.Tests.E2E/Pages/<Page>Tests.cs`.

- Classes extend `TestBase(fixture)`; each test opens its own page with `CreatePageAsync()`, since tests run in parallel.
- Name tests `Page_Feature_Expectation`; the no-JS path uses `_No_JS_` and `CreatePageAsync(isJsEnabled: false)`.
- Assert with AwesomeAssertions, against `AppConstants` where the value lives there.
