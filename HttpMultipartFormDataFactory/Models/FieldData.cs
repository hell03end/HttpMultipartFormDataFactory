namespace HttpMultipartFormDataFactory.Models;

internal struct FieldData
{
    public HttpContent Value { get; set; }

    public string ParamName { get; set; }

    public string? FileName { get; set; }
}
