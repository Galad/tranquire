using Tranquire.Selenium.Targets;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Creates targets
    /// </summary>
    public static class Target
    {
        /// <summary>
        /// Creates a new target.
        /// </summary>
        /// <param name="friendlyName">A friendly name for the target</param>
        /// <returns>A <see cref="TargetBuilder"/> used to configure the target</returns>
        public static TargetBuilder The(string friendlyName)
        {
            return new TargetBuilder(friendlyName);
        }
    }
}
