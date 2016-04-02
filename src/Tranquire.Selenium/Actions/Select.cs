using Tranquire.Selenium.Actions.Selects;

namespace Tranquire.Selenium.Actions
{
    public static class Select
    { 
        public static SelectBuilder From(ITarget target)
        {
            return new SelectBuilder(target);
        }        
    }
}