

using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium
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

    public class HitId : IAction
    {
        private readonly string _id;
        private readonly string _keys;

        public HitId(string id, string keys)
        {
            _id = id;
            _keys = keys;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.AbilityTo<BrowseTheWeb>().Driver.FindElement(By.Id(_id)).SendKeys(_keys);
            return actor;
        }
    }
}
