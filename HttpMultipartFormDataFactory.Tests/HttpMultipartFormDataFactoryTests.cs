using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpMultipartFormDataFactory.Tests;

[TestClass]
public class HttpMultipartFormDataFactoryTests
{
    private static CancellationToken Token { get; } = CancellationToken.None;

    private static HttpMultipartFormDataFactory HttpMultipartFormDataFactory { get; } =
        HttpMultipartFormDataFactory.Default;

    private static IFormFile File { get; } =
        new FormFile(new MemoryStream(Encoding.Default.GetBytes("test")), 0, 4, "file", "file.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain",
            ContentDisposition = "form-data; name=\"File\"; filename=\"file.txt\"",
        };

    #region TestValues

    private const object Null = null!;
    private const string String = "string";
    private const long Long = long.MaxValue;
    private const int Int = int.MaxValue;
    private const char Char = char.MaxValue;
    private const bool Bool = true;
    private const float Float = float.MaxValue;
    private const double Double = double.MaxValue;
    private const byte Byte = byte.MaxValue;
    private const decimal Decimal = decimal.MaxValue;

    private static readonly IFormFile[] Files = { File, File, File, };
    private static readonly string[] Strings = { String, String, String, };
    private static readonly long[] Longs = { Long, Long, Long, };
    private static readonly int[] Ints = { Int, Int, Int, };
    private static readonly char[] Chars = { Char, Char, Char, };
    private static readonly bool[] Bools = { Bool, Bool, Bool, };
    private static readonly float[] Floats = { Float, Float, Float, };
    private static readonly double[] Doubles = { Double, Double, Double, };
    private static readonly byte[] Bytes = { Byte, Byte, Byte, };
    private static readonly decimal[] Decimals = { Decimal, Decimal, Decimal, };

    #endregion TestValues

    [TestMethod]
    public async Task MultipartDataContentCreatedNull()
    {
        var request = new
        {
            Null,
        };

        var content = await HttpMultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(!content.Any()); // null values should be filtered
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedSingleFile()
    {
        var request = new
        {
            File,
        };

        var content = await HttpMultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());
        Assert.IsTrue(content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(File)}"))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreatedMultipleFile()
    {
        var request = new
        {
            Files,
        };

        var content = await HttpMultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());
        Assert.IsTrue(content.All(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(File)}"))));
    }

    [TestMethod]
    public async Task MultipartDataContentCreated()
    {
        var request = new
        {
            String,
            EmptyString = string.Empty,
            Long,
            Int,
            Char,
            Bool,
            Float,
            Double,
            Byte,
            Decimal,
        };

        var content = await HttpMultipartFormDataFactory.Create(request, Token);

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
            Strings,
            Longs,
            Ints,
            Chars,
            Bools,
            Floats,
            Doubles,
            Bytes,
            Decimals,
        };

        var content = await HttpMultipartFormDataFactory.Create(request, Token);

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
            Null,
            File,
            Files,
            String,
            EmptyString = string.Empty,
            Long,
            Int,
            Char,
            Bool,
            Float,
            Double,
            Byte,
            Decimal,
            Strings,
            Longs,
            Ints,
            Chars,
            Bools,
            Floats,
            Doubles,
            Bytes,
            Decimals,
        };

        var content = await HttpMultipartFormDataFactory.Create(request, Token);

        Assert.IsNotNull(content);
        Assert.IsTrue(content.Any());

        var names = request.GetType().GetProperties()
            .Select(x => x.Name)
            .Where(x => !x.Equals(nameof(Null), StringComparison.OrdinalIgnoreCase))
            .ToArray();

        Assert.IsTrue(names.All(n => content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={n}")))));

        Assert.IsFalse(content.Any(x => x.Headers.Any(h => h.Value.First().Contains($"name={nameof(Null)}"))));
    }
}
