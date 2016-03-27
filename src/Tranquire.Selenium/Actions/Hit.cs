

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
        private readonly string _keys;

        public Hit(string keys) : base(t => new EnterValue(keys, t))
        {
            _keys = keys;
        }

        /// <summary>
        /// Creates an action which hit the Enter key
        /// </summary>
        /// <returns></returns>
        public static Hit Enter()
        {
            return new Hit(Keys.Enter);
        }
    }   
}
