using System;
using System.IO;
using System.Linq;
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
    public async Task MultipartDataContentCreatedNull()
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
    public async Task MultipartDataContentCreatedSingleFile()
    {
        var request = new
        {
            TestValues.File,
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());
        Assert.IsTrue(content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(File)}"))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedMultipleFile()
    {
        var request = new
        {
            TestValues.Files,
        };

        var content = await MultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());
        Assert.IsTrue(content.All(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(File)}"))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreated()
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

        var names = request.GetType().GetProperties()
            .Select(x => x.Name)
            .ToArray();

        Assert.IsTrue(names.All(n => content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={n}")))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedCollections()
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

        var names = request.GetType().GetProperties()
            .Select(x => x.Name)
            .ToArray();

        Assert.IsTrue(names.All(n => content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={n}")))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedMixed()
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

        var names = request.GetType().GetProperties()
            .Select(x => x.Name)
            .Where(x => !x.Equals(nameof(TestValues.Null), StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.IsTrue(names.All(n => content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={n}")))));
    }

    [TestMethod]
    public async Task MultipartDataContentWithCacheNotFail()
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
            Assert.IsTrue(content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(File)}"))));
        }
    }

    [TestMethod]
    public async Task MultipartDataContentWithoutCacheNotFail()
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
            Assert.IsTrue(content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(File)}"))));
        }
    }
}
