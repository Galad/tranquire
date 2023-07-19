using System;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium;

/// <summary>
/// Extensions for <see cref="SeleniumReportingConfiguration"/>
/// </summary>
public static class SeleniumReportingConfigurationExtensions
{
    /// <summary>
    /// Save the screenshots in the jpeg format
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static SeleniumReportingConfiguration WithScreenshotJpeg(this SeleniumReportingConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration.WithScreenshotFormat(ScreenshotFormat.Jpeg);
    }

    /// <summary>
    /// Save the screenshots in the png format
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static SeleniumReportingConfiguration WithScreenshotPng(this SeleniumReportingConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration.WithScreenshotFormat(ScreenshotFormat.Png);
    }

    /// <summary>
    /// Save the screenshots in the tiff format
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static SeleniumReportingConfiguration WithScreenshotTiff(this SeleniumReportingConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration.WithScreenshotFormat(ScreenshotFormat.Tiff);
    }

    /// <summary>
    /// Save the screenshots in the gif format
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static SeleniumReportingConfiguration WithScreenshotGif(this SeleniumReportingConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration.WithScreenshotFormat(ScreenshotFormat.Gif);
    }

    /// <summary>
    /// Save the screenshots in the bmp format
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static SeleniumReportingConfiguration WithScreenshotBmp(this SeleniumReportingConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration.WithScreenshotFormat(ScreenshotFormat.Bmp);
    }
}