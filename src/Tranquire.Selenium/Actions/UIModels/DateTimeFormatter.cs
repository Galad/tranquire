using System;
using System.Text.RegularExpressions;

namespace Tranquire.Selenium.Actions.UIModels;

internal static class DateTimeFormatter
{
    private static readonly char[] HourTokens = { 'h', 'H' };
    private static readonly Regex DateFormatPattern = new(
        @"\A[dMyHhmsft/\-.:, ]+\z",
        RegexOptions.None,
        TimeSpan.FromSeconds(1));

    /// <summary>Maps a date placeholder such as <c>DD/MM/YYYY</c> to a .NET format, or returns null when it is not a date pattern.</summary>
    public static string MapPlaceholderToDotNetFormat(string placeholder)
    {
        if (string.IsNullOrEmpty(placeholder))
            return null;

        var format = placeholder
            .Replace("YYYY", "yyyy")
            .Replace("YY", "yy")
            .Replace("DD", "dd")
            .Replace("D", "d");
        // Without an hour component, lower-case "m" means month (as in "dd/mm/yyyy"), not minutes.
        if (format.IndexOfAny(HourTokens) < 0)
            format = format.Replace('m', 'M');
        return DateFormatPattern.IsMatch(format) ? format : null;
    }
}
