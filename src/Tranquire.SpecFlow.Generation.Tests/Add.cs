using System;
using Tranquire.Extensions;

namespace Tranquire.SpecFlow.Generation.Tests
{
    [GenerateSpecFlowBindings]
    public static class Add
    {
        internal static IAction<Unit> ParameterlessMethodAction = Actions.Create("Test", a => Console.WriteLine("test"));
        internal static Func<string, IAction<Unit>> MethodWithOneParameterAction = s => Actions.Create("Test", a => Console.WriteLine(s));
        internal static Func<string, int, object, IAction<Unit>> MethodWithMultipleParametersAction = (s, i, o) => Actions.Create("Test", a => Console.WriteLine(s));

        public static IAction<Unit> ParameterlessMethod()
        {
            return ParameterlessMethodAction;
        }

        public static IAction<Unit> MethodWithOneParameter(string parameter)
        {
            return MethodWithOneParameterAction(parameter);
        }

        public static IAction<Unit> MethodWithMultipleParameters(string parameter1, int parameter2, object parameter3)
        {
            return MethodWithMultipleParametersAction(parameter1, parameter2, parameter3);
        }
    }
}
