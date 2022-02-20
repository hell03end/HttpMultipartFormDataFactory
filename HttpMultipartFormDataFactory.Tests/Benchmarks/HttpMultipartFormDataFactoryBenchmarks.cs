using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace HttpMultipartFormDataFactory.Tests.Benchmarks
{
    [MemoryDiagnoser]
    public class HttpMultipartFormDataFactoryBenchmarks
    {
        private static CancellationToken Token { get; } = CancellationToken.None;
        private MultipartFormDataFactory MultipartFormDataFactory { get; } = new();
        private MultipartFormDataFactory MultipartFormDataFactoryWithoutCache { get; } = new(false);

        [Benchmark]
        public async Task<MultipartFormDataContent[]> CreateWithCache()
        {
            var request = new
            {
                TestValues.Null,
                TestValues.File,
                TestValues.Files,
                TestValues.String,
                TestValues.EmptyString,
                TestValues.Long,
                TestValues.Int,
                TestValues.Char,
                TestValues.Bool,
                TestValues.Float,
                TestValues.Double,
                TestValues.Byte,
                TestValues.Decimal,
                TestValues.Guid,
                TestValues.DateTimeOffset,
                TestValues.Nulls,
                TestValues.Strings,
                TestValues.Longs,
                TestValues.Ints,
                TestValues.Chars,
                TestValues.Bools,
                TestValues.Floats,
                TestValues.Doubles,
                TestValues.Bytes,
                TestValues.Decimals,
                TestValues.Guids,
                TestValues.DateTimeOffsets,
                TestValues.EmptyStrings,
            };

            return await Task.WhenAll(
                Enumerable.Range(0, 10).Select(_ => MultipartFormDataFactory.Create(request, Token)));
        }

        [Benchmark]
        public async Task<MultipartFormDataContent[]> CreateWithoutCache()
        {
            var request = new
            {
                TestValues.Null,
                TestValues.File,
                TestValues.Files,
                TestValues.String,
                TestValues.EmptyString,
                TestValues.Long,
                TestValues.Int,
                TestValues.Char,
                TestValues.Bool,
                TestValues.Float,
                TestValues.Double,
                TestValues.Byte,
                TestValues.Decimal,
                TestValues.Guid,
                TestValues.DateTimeOffset,
                TestValues.Nulls,
                TestValues.Strings,
                TestValues.Longs,
                TestValues.Ints,
                TestValues.Chars,
                TestValues.Bools,
                TestValues.Floats,
                TestValues.Doubles,
                TestValues.Bytes,
                TestValues.Decimals,
                TestValues.Guids,
                TestValues.DateTimeOffsets,
                TestValues.EmptyStrings,
            };

            return await Task.WhenAll(
                Enumerable.Range(0, 10).Select(_ => MultipartFormDataFactoryWithoutCache.Create(request, Token)));
        }
    }
}
