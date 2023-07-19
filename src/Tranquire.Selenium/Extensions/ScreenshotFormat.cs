using OpenQA.Selenium;

namespace Tranquire.Selenium.Extensions;

/// <summary>
/// Represent the format of a screenshot
/// </summary>
public sealed class ScreenshotFormat
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static ScreenshotFormat Jpeg { get; } = new ScreenshotFormat(ScreenshotImageFormat.Jpeg);
    public static ScreenshotFormat Png { get; } = new ScreenshotFormat(ScreenshotImageFormat.Png);
    public static ScreenshotFormat Bmp { get; } = new ScreenshotFormat(ScreenshotImageFormat.Bmp);
    public static ScreenshotFormat Tiff { get; } = new ScreenshotFormat(ScreenshotImageFormat.Tiff);
    public static ScreenshotFormat Gif { get; } = new ScreenshotFormat(ScreenshotImageFormat.Gif);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    private ScreenshotFormat(ScreenshotImageFormat format)
    {
        Format = format;
    }

    /// <summary>
    /// Gets the format
    /// </summary>
    public ScreenshotImageFormat Format { get; }

    /// <summary>
    /// Gets the associated extension
    /// </summary>
    public string Extension => "." + Format.ToString().ToLower();
}
