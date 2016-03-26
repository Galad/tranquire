

using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Actions.Hits;

namespace Tranquire.Selenium.Actions
{
    public class Hit : TargetableAction<HitAction>
    {
        private readonly string _keys;

        public Hit(string keys) : base(t => new HitAction(t, keys))
        {
            _keys = keys;
        }

        public static Hit Enter()
        {
            return new Hit(Keys.Enter);
        }
    }   
}
