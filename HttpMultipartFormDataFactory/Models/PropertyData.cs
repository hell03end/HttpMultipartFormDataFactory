using System.Reflection;

namespace HttpMultipartFormDataFactory.Models;

internal struct PropertyData
{
    public PropertyInfo? PropertyInfo { get; set; }

    public bool IsFile { get; set; }

    public bool IsCollection { get; set; }

    public string? Name { get; set; }
}
