namespace Tranquire.Reporting;

/// <summary>
/// Represents the outcome of a verification
/// </summary>
public enum ThenOutcome
{
    /// <summary>
    /// The verification pass
    /// </summary>
    Pass,
    /// <summary>
    /// The verification failed
    /// </summary>
    Failed,
    /// <summary>
    /// The verification thrown an exception
    /// </summary>
    Error
}