using BenchmarkDotNet.Running;

namespace HttpMultipartFormDataFactory.Tests.Benchmarks
{
    public static class Program
    {
        public static void Main() => BenchmarkRunner.Run<HttpMultipartFormDataFactoryBenchmarks>();
    }
}
