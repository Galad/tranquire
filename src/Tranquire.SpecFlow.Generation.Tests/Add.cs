using System;
using Tranquire.Extensions;

namespace Tranquire.SpecFlow.Generation.Tests
{
    [GenerateSpecFlowBindings]
    public static class Add
    {
        internal static IAction<Unit> ParameterlessMethodAction = Actions.Create("Test", a => Console.WriteLine("test"));
        internal static Func<string, IAction<Unit>> MethodWithOneParameterAction = s => Actions.Create("Test", a => Console.WriteLine(s));

        public static IAction<Unit> ParameterlessMethod()
        {
            return ParameterlessMethodAction;
        }

        public static IAction<Unit> MethodWithOneParameter(string parameter)
        {
            return MethodWithOneParameterAction(parameter);
        }
    }
}
