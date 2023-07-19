using System;
using BenchmarkDotNet.Running;

namespace Benchmarks;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Invalid number of arguments");
            return;
        }
        var flag = args.Length == 0 ? "-r" : args[0];

        if (flag == "-r")
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
        else if (flag == "-d")
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
