using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Tranquire;

namespace Benchmarks;

[MemoryDiagnoser]
[Q1Column, MedianColumn, Q3Column]
[EtwProfiler]
public class DeepActionExecutionBenchmark
{
    private readonly int[] _ints;
    private readonly Actor _actor;
    private readonly Actor _actorReporting;
    private readonly DeepAction _deepAction;
    private readonly SumClass _sumClass = new(1);

    public DeepActionExecutionBenchmark()
    {
        _ints = Enumerable.Range(0, 10).ToArray();
        _actor = new Actor("John");
        _actorReporting = new Actor("Bob").WithReporting(new Tranquire.Reporting.XmlDocumentObserver());
        _deepAction = new DeepAction(1, _ints);
    }

    private static IAction<Unit> SumUnit(int[] values) => Actions.Create("sum", a => { values.Sum(); });
    private static IAction<int> Sum(int[] values) => Actions.Create("sum", a => values.Sum());
    private const int MaxDepth = 10;

    [Benchmark]
    public int Tranquire_OnTheFlyActions()
    {
        return _actor.Given(new DeepActionOnTheFly(1, _ints));
    }

    private class DeepActionOnTheFly : ActionBase<int>
    {
        private readonly int _depth;
        private readonly int[] _ints;

        public DeepActionOnTheFly(int depth, int[] ints)
        {
            _depth = depth;
            _ints = ints;
        }

        public override string Name => "action";

        protected override int ExecuteWhen(IActor actor)
        {
            if (_depth == MaxDepth)
            {
                return _ints.Sum();
            }
            return actor.Execute(new DeepActionOnTheFly(_depth + 1, _ints));
        }
    }

    [Benchmark]
    public int Tranquire_OnTheFlyActions_Instanciated()
    {
        return _actor.Given(_deepAction);
    }

    [Benchmark]
    public int Tranquire_OnTheFlyActions_Instanciated_WithReporting()
    {
        return _actorReporting.Given(_deepAction);
    }

    private class DeepAction : ActionBase<int>
    {
        private readonly int _depth;
        private readonly int[] _ints;
        private readonly DeepAction _next;

        public DeepAction(int depth, int[] ints)
        {
            _depth = depth;
            _ints = ints;
            _next = depth < MaxDepth ? new DeepAction(depth + 1, ints) : null;
        }

        public override string Name => "action";

        protected override int ExecuteWhen(IActor actor)
        {
            if (_next is null)
            {
                return _ints.Sum();
            }
            return actor.Execute(_next);
        }
    }

    [Benchmark(Baseline = true)]
    public int Objects()
    {
        return _sumClass.Sum(_ints);
    }

    private class SumClass
    {
        private readonly int _depth;
        private readonly SumClass _next;

        public SumClass(int depth)
        {
            _depth = depth;
            _next = depth < MaxDepth ? new SumClass(depth + 1) : null;
        }

        public int Sum(int[] values)
        {
            if (_next is null)
            {
                return values.Sum();
            }
            return _next.Sum(values);
        }
    }
}
