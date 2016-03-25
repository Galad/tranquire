

using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Actions.Hits;

namespace Tranquire.Selenium.Actions
{
    public class Hit
    {
        private readonly string _keys;

        public Hit(string keys)
        {
            _keys = keys;
        }

        public static Hit Enter()
        {
            return new Hit(Keys.Enter);
        }

        public HitId Into(string id)
        {
            return new HitId(id, _keys);
        }
    }   
}
