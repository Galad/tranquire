

using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Actions.Enters;

namespace Tranquire.Selenium.Actions
{
    public class Hit : TargetableAction<EnterValue>
    {
        private readonly string _keys;

        public Hit(string keys) : base(t => new EnterValue(keys, t))
        {
            _keys = keys;
        }

        public static Hit Enter()
        {
            return new Hit(Keys.Enter);
        }
    }   
}
