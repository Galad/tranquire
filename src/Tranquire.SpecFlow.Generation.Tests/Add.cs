using System;
using Tranquire.Extensions;

namespace Tranquire.SpecFlow.Generation.Tests
{
    [GenerateSpecFlowBindings]
    public static class Add
    {
        internal static IAction<Unit> ParameterlessMethodAction = Actions.Create("Test", a => Console.WriteLine("test"));

        public static IAction<Unit> ParameterlessMethod()
        {
            return ParameterlessMethodAction;
        }
    }
}
