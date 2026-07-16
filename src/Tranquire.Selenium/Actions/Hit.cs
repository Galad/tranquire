using Tranquire.Selenium.Actions.Enters;

namespace Tranquire.Selenium.Actions;

/// <summary>
/// Creates key hit actions
/// </summary>
public class Hit : TargetableAction<EnterValue>
{
    /// <summary>
    /// Gets the keys to hit
    /// </summary>
    public string HitKeys { get; }

    /// <inheritdoc />
    public string Name => $"Hit the keys {HitKeys}";

    /// <summary>
    /// Creates a new instance of <see cref="Hit"/>
    /// </summary>
    /// <param name="keys">The keys to hit</param>
    public Hit(string keys) : base(t => new EnterValue(keys, t))
    {
        HitKeys = keys;
    }

    /// <summary>
    /// Creates an action which hit the Enter key
    /// </summary>
    /// <returns></returns>
    public static Hit Enter()
    {
        return new Hit(OpenQA.Selenium.Keys.Enter);
    }

    /// <summary>
    /// Creates an action which hit the Enter key
    /// </summary>
    /// <returns></returns>
    public static Hit Escape()
    {
        return new Hit(OpenQA.Selenium.Keys.Escape);
    }

    /// <summary>
    /// Creates an action which hit the Enter key
    /// </summary>
    /// <returns></returns>
    public static Hit Keys(string keys)
    {
        return new Hit(keys);
    }
}
