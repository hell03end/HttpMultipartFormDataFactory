using System.Reflection;

namespace HttpMultipartFormDataFactory.Models;

public struct PropertyData
{
    public PropertyInfo? PropertyInfo { get; set; }

    public bool IsFile { get; set; }

    public bool IsCollection { get; set; }

    public string? Name { get; set; }
}
