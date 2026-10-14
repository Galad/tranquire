using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire.Selenium.Actions.UIModels;

internal static class DateTimeFormatter
{
    public static string MapPlaceholderToDotNetFormat(string placeholder)
    {
        if (string.IsNullOrEmpty(placeholder))
            return placeholder;

        return placeholder
            .Replace("YYYY", "yyyy")
            .Replace("YY", "yy")
            .Replace("DD", "dd")
            .Replace("D", "d");
    }
}
