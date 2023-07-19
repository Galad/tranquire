using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Tranquire;

namespace Benchmarks;

[MemoryDiagnoser]
[Q1Column, MedianColumn, Q3Column]
[EtwProfiler]
public class ActionsExecutionBenchmark
{
    private readonly int[] _ints;
    private readonly Actor _actor;
    private readonly IAction<Unit> _sumUnit;
    private readonly IAction<int> _sum;
    private readonly SumClass _sumClass = new();

    public ActionsExecutionBenchmark()
    {
        _ints = Enumerable.Range(0, 10).ToArray();
        _actor = new Actor("John");
        _sumUnit = SumUnit(_ints);
        _sum = Sum(_ints);
        _selectManySum = _ => _sum;
    }

    private static IAction<Unit> SumUnit(int[] values) => Actions.Create("sum", a => { values.Sum(); });
    private static IAction<int> Sum(int[] values) => Actions.Create("sum", a => values.Sum());

    [Benchmark]
    public Unit Tranquire_OnTheFlyActions_Composite()
    {
        return _actor.Given(
            new DefaultCompositeAction("actions",
            SumUnit(_ints),
            SumUnit(_ints),
            SumUnit(_ints),
            SumUnit(_ints)
            ));
    }

    [Benchmark]
    public int Tranquire_OnTheFlyActions_SelectMany()
    {
        IAction<int> action = Sum(_ints).SelectMany(_ => Sum(_ints))
                                        .SelectMany(_ => Sum(_ints))
                                        .SelectMany(_ => Sum(_ints));
        return _actor.Given(action);
    }

    [Benchmark]
    public Unit Tranquire_InstanciatedActions_Composite()
    {
        return _actor.Given(
               new DefaultCompositeAction("actions",
               _sumUnit,
               _sumUnit,
               _sumUnit,
               _sumUnit
               ));
    }

    [Benchmark]
    public int Tranquire_InstanciatedActions_SelectMany()
    {
        IAction<int> action = _sum.SelectMany(_ => _sum)
                                .SelectMany(_ => _sum)
                                .SelectMany(_ => _sum);
        return _actor.Given(action);
    }

    private Func<int, IAction<int>> _selectManySum;
    [Benchmark]
    public int Tranquire_InstanciatedFunc_SelectMany()
    {
        IAction<int> action = _sum.SelectMany(_ => _sum)
                                .SelectMany(_ => _sum)
                                .SelectMany(_ => _sum);
        return _actor.Given(action);
    }

    private class SumClass
    {
        public int Sum(int[] values) => values.Sum();
    }

    [Benchmark]
    public int Objects()
    {
        return _sumClass.Sum(_ints) + _sumClass.Sum(_ints) + _sumClass.Sum(_ints) + _sumClass.Sum(_ints);
    }

    [Benchmark(Baseline = true)]
    public int Inline()
    {
        return _ints.Sum() + _ints.Sum() + _ints.Sum() + _ints.Sum();
    }
}
