namespace Tranquire.Selenium;

/// <summary>
/// Represent a target taking parameters
/// </summary>
public interface ITargetWithParameters
{
    /// <summary>
    /// Returns a target with the given parameters
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    ITarget Of(params object[] parameters);
}

/// <summary>
/// Represent a target taking parameters
/// </summary>
public interface ITargetWithParameters<in T>
{
    /// <summary>
    /// Returns a target with the given parameters
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    ITarget Of(T parameter);
}
