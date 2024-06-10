using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace SmoothNanners.Web.Vite;

/// <summary>
/// Tag helper to get hashed asset file name URL from Vite manifest.
/// </summary>
/// <remarks>
/// Using the manifest is necessary over "asp-append-version" because the latter uses the "?v=" format to bust cache,
/// which does not resolve the URL when deployed on GitHub pages as a static website.
/// </remarks>
[HtmlTargetElement("link", Attributes = ViteAssetAttributeName, TagStructure = TagStructure.WithoutEndTag)]
[HtmlTargetElement("script", Attributes = ViteAssetAttributeName)]
public sealed class ViteAssetTagHelper : TagHelper
{
    private const string ViteAssetAttributeName = "vite-asset";
    private const string CacheKey = "vite:assets";
    private readonly IMemoryCache _cache;
    private readonly IFileProvider _fileProvider;
    private readonly string _publicAssetsDirectoryName;
    private readonly IOptions<ViteOptions> _viteOptions;

    public ViteAssetTagHelper(IMemoryCache cache, IWebHostEnvironment environment, IOptions<ViteOptions> viteOptions)
    {
        _cache = cache;
        _fileProvider = environment.WebRootFileProvider;
        _viteOptions = viteOptions;

#pragma warning disable CA1308
        _publicAssetsDirectoryName = _viteOptions.Value.AssetsDirectoryName.ToLowerInvariant();
#pragma warning restore CA1308
    }

    [HtmlAttributeName(ViteAssetAttributeName)]
    public required string ViteAsset { get; set; }

    [ViewContext]
    public required ViewContext ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var assets = await _cache.GetOrCreateAsync<IDictionary<string, Asset>>(
            CacheKey,
            async entry =>
            {
                var filePath = Path.Combine(_publicAssetsDirectoryName, _viteOptions.Value.ManifestFileName);
                var fileInfo = _fileProvider.GetFileInfo(filePath);

                if (!fileInfo.Exists || fileInfo.IsDirectory)
                {
                    throw new FileNotFoundException($"Vite manifest file not found at path [{filePath}].");
                }

                entry.AddExpirationToken(_fileProvider.Watch(filePath));

                await using var fileStream = fileInfo.CreateReadStream();

                var result = await JsonSerializer.DeserializeAsync<IDictionary<string, Asset>>(
                    fileStream,
                    cancellationToken: ViewContext.HttpContext.RequestAborted);

                return result!;
            });

        if (!assets!.TryGetValue($"{_viteOptions.Value.AssetsDirectoryName}/{ViteAsset}", out var asset))
        {
            throw new InvalidOperationException($"Asset [{ViteAsset}] not found.");
        }

        var attributeName = output.TagName == "link" ? "href" : "src";
        var assetPath = $"/{_publicAssetsDirectoryName}/{asset.FilePath}";

        output.Attributes.SetAttribute(attributeName, assetPath);
    }

    private sealed record Asset(
        [property: JsonPropertyName("file")]
        string FilePath);
}
