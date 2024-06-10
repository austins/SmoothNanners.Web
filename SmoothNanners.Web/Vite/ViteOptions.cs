using System.ComponentModel.DataAnnotations;

namespace SmoothNanners.Web.Vite;

public sealed class ViteOptions : IValidatableObject
{
    public const string SectionName = "Vite";

    /// <summary>
    /// The directory for the raw assets relative to the root of the project.
    /// </summary>
    [Required]
    public required string AssetsDirectoryName { get; init; }

    /// <summary>
    /// The paths for the raw assets relative to the <see cref="AssetsDirectoryName" />.
    /// </summary>
    [Required]
    [MinLength(1)]
    public required ISet<string> AssetPaths { get; init; }

    /// <summary>
    /// The Vite manifest file name.
    /// </summary>
    [Required]
    public required string ManifestFileName { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AssetsDirectoryName.Contains(Path.DirectorySeparatorChar, StringComparison.Ordinal)
            || AssetsDirectoryName.Contains(Path.AltDirectorySeparatorChar, StringComparison.Ordinal))
        {
            yield return new ValidationResult(
                $"{nameof(AssetsDirectoryName)} must be a name of a directory in the root of the project.",
                [nameof(AssetsDirectoryName)]);
        }

        if (AssetPaths.Any(x => x.Contains("..", StringComparison.Ordinal)))
        {
            yield return new ValidationResult(
                $"{nameof(AssetPaths)} must not go up to parent directories.",
                [nameof(AssetPaths)]);
        }

        if (ManifestFileName.Contains(Path.DirectorySeparatorChar, StringComparison.Ordinal)
            || ManifestFileName.Contains(Path.AltDirectorySeparatorChar, StringComparison.Ordinal))
        {
            yield return new ValidationResult(
                $"{nameof(ManifestFileName)} must be a file name and not be a path.",
                [nameof(ManifestFileName)]);
        }
    }
}
