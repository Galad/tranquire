namespace Tranquire.Selenium.Actions.UIModels;

internal static class XPathStringLiteral
{
    public static string Format(string value)
    {
        value ??= string.Empty;

        if (value.IndexOf('\'') < 0)
            return $"'{value}'";
        if (value.IndexOf('"') < 0)
            return $"\"{value}\"";

        var parts = value.Split('\'');
        var literalParts = new string[parts.Length * 2 - 1];
        for (var index = 0; index < parts.Length; index++)
        {
            literalParts[index * 2] = $"'{parts[index]}'";
            if (index < parts.Length - 1)
                literalParts[index * 2 + 1] = "\"'\"";
        }

        return $"concat({string.Join(", ", literalParts)})";
    }
}