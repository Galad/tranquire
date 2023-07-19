namespace Tranquire.Selenium;

/// <summary>
/// Represent an object using a <see cref="ITarget"/>
/// </summary>
public interface ITargeted
{
    /// <summary>
    /// Gets the target
    /// </summary>
    ITarget Target { get; }
}
