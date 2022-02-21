using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpMultipartFormDataFactory.Tests;

[TestClass]
public class MultipartFormDataFactoryTests
{
    private static CancellationToken Token { get; } = CancellationToken.None;

    private static MultipartFormDataFactory MultipartFormDataFactory { get; } = MultipartFormDataFactory.Default;

    [TestMethod]
    public async Task MultipartDataContentCreatedFromNull()
    {
        var request = new
        {
            TestValues.Null,
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(!content.Any()); // null values should be filtered
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedFromSingleFile()
    {
        var request = new
        {
            TestValues.File,
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());
        Assert.IsTrue(content.Any(x => CheckHeadersContains(x, nameof(File))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedFromMultipleFiles()
    {
        var request = new
        {
            TestValues.Files,
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());
        Assert.IsTrue(content.All(x => CheckHeadersContains(x, nameof(File))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedFromSingleValues()
    {
        var request = new
        {
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
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());

        var names = GetNames(request, true);

        Assert.IsTrue(names.All(n => content.Any(x => CheckHeadersContains(x, n))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedFromCollections()
    {
        var request = new
        {
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
            TestValues.Nulls,
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());

        var names = GetNames(request, true);

        Assert.IsTrue(names.All(n => content.Any(x => CheckHeadersContains(x, n))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedFromMixed()
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

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());

        var names = GetNames(request, false);

        Assert.IsTrue(names.All(n => content.Any(x => CheckHeadersContains(x, n))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedWithCacheNotFail()
    {
        var multipartFormDataFactory = new MultipartFormDataFactory();
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

        var results =
            await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => multipartFormDataFactory.Create(request, Token)));

        foreach (var content in results)
        {
            Assert.IsNotNull(content);
            Assert.IsTrue(content.Any());

            var names = GetNames(request, false);

            Assert.IsTrue(names.All(n => content.Any(x => CheckHeadersContains(x, n))));
        }
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedWithoutCacheNotFail()
    {
        var multipartFormDataFactory = new MultipartFormDataFactory(false);
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

        var results =
            await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => multipartFormDataFactory.Create(request, Token)));

        foreach (var content in results)
        {
            Assert.IsNotNull(content);
            Assert.IsTrue(content.Any());

            var names = GetNames(request, false);

            Assert.IsTrue(names.All(n => content.Any(x => CheckHeadersContains(x, n))));
        }
    }

    private static bool CheckHeadersContains(HttpContent content, string value) =>
        content.Headers.Any(h => CheckHeadersContains(h, value));

    private static bool CheckHeadersContains(KeyValuePair<string, IEnumerable<string>> headers, string value) =>
        headers.Value.Any(x => x.Contains($"name={value}"));

    private static string[] GetNames(object request, bool withNulls) => request.GetType().GetProperties()
        .Select(x => x.Name)
        .Where(x => withNulls || !x.Equals(nameof(TestValues.Null), StringComparison.OrdinalIgnoreCase))
        .ToArray();
}
