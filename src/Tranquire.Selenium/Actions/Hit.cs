

using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Actions.Enters;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Creates key hit actions
    /// </summary>
    public class Hit : TargetableAction<EnterValue>
    {
        /// <summary>
        /// Gets the keys to hit
        /// </summary>
        public string Keys { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Hit"/>
        /// </summary>
        /// <param name="keys">The keys to hit</param>
        public Hit(string keys) : base(t => new EnterValue(keys, t))
        {
            Keys = keys;
        }

        /// <summary>
        /// Creates an action which hit the Enter key
        /// </summary>
        /// <returns></returns>
        public static Hit Enter()
        {
            return new Hit(OpenQA.Selenium.Keys.Enter);
        }
    }   
}
