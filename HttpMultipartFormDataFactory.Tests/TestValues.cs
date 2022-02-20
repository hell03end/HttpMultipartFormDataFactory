using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HttpMultipartFormDataFactory.Tests;

internal static class TestValues
{
    public static IFormFile File { get; } =
        new FormFile(new MemoryStream(Encoding.Default.GetBytes("test")), 0, 4, "file", "file.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain",
            ContentDisposition = "form-data; name=\"File\"; filename=\"file.txt\"",
        };

    public const object? Null = null!;
    public static string String { get; } = Guid.NewGuid().ToString();
    public static string EmptyString => string.Empty;
    public const long Long = long.MaxValue;
    public const int Int = int.MaxValue;
    public const char Char = char.MaxValue;
    public const bool Bool = true;
    public const float Float = float.MaxValue;
    public const double Double = double.MaxValue;
    public const byte Byte = byte.MaxValue;
    public const decimal Decimal = decimal.MaxValue;
    public static Guid Guid { get; } = Guid.NewGuid();
    public static DateTimeOffset DateTimeOffset { get; } = DateTimeOffset.UtcNow;

    public static object?[] Nulls { get; } = { Null, Null, Null };
    public static IFormFile[] Files { get; } = { File, File, File, };
    public static string[] Strings { get; } = { String, String, String, };
    public static string[] EmptyStrings { get; } = { EmptyString, EmptyString, EmptyString, };
    public static long[] Longs { get; } = { Long, Long, Long, };
    public static int[] Ints { get; } = { Int, Int, Int, };
    public static char[] Chars { get; } = { Char, Char, Char, };
    public static bool[] Bools { get; } = { Bool, Bool, Bool, };
    public static float[] Floats { get; } = { Float, Float, Float, };
    public static double[] Doubles { get; } = { Double, Double, Double, };
    public static byte[] Bytes { get; } = { Byte, Byte, Byte, };
    public static decimal[] Decimals { get; } = { Decimal, Decimal, Decimal, };
    public static Guid[] Guids { get; } = { Guid, Guid, Guid, };
    public static DateTimeOffset[] DateTimeOffsets { get; } = { DateTimeOffset, DateTimeOffset, DateTimeOffset, };
}
